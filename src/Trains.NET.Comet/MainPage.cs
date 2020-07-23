﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Comet;
using Trains.NET.Engine;
using Trains.NET.Engine.Tracks;
using Trains.NET.Rendering;
using Trains.NET.Rendering.Tracks;

namespace Trains.NET.Comet
{
    public class MainPage : View
    {
        private readonly State<bool> _configurationShown = false;
        private readonly IGame _game;
        private readonly IPixelMapper _pixelMapper;
        private readonly ITrackParameters _trackParameters;
        private readonly ITrackLayout _trackLayout;
        private readonly IGameStorage _gameStorage;
        private readonly MiniMapDelegate _miniMapDelegate;
        private readonly ITimer _gameTimer;
        private Size _lastSize = Size.Empty;

        public MainPage(IGame game,
                        IPixelMapper pixelMapper,
                        OrderedList<ITool> tools,
                        OrderedList<ILayerRenderer> layers,
                        OrderedList<ICommand> commands,
                        ITrainController trainControls,
                        ITrackParameters trackParameters,
                        ITrackLayout trackLayout,
                        IGameStorage gameStorage,
                        ITimer gameTimer,
                        Factory<IToolPreviewer> previewerFactory)
        {
            this.Title("Trains - " + ThisAssembly.AssemblyInformationalVersion);

            var controlDelegate = new TrainsDelegate(game, pixelMapper, previewerFactory);
            _miniMapDelegate = new MiniMapDelegate(trackLayout, trackParameters, pixelMapper);

            this.Body = () =>
            {
                return new HStack()
                {
                    new VStack()
                    {
                        _configurationShown ? null :
                            new Button(trainControls.BuildMode ? "Building" : "Playing", ()=> SwitchGameMode()),
                        new Spacer(),
                        _configurationShown ?
                                CreateConfigurationControls(layers) :
                                CreateToolsControls(tools, controlDelegate, trainControls.BuildMode.Value),
                        new Spacer(),
                        _configurationShown || !trainControls.BuildMode ? null :
                            CreateCommandControls(commands),
                        new Spacer(),
                        new HStack()
                        {
                            new Button("      +      ", ()=> Zoom(1))
                                .FillHorizontal(),
                            new Button("      -      ", ()=> Zoom(-1))
                                .FillHorizontal(),
                        }.FillHorizontal(),
                        new Spacer(),
                        new Button("Configuration", ()=> _configurationShown.Value = !_configurationShown.Value),
                        new DrawableControl(_miniMapDelegate).Frame(height: 100)
                    }.Frame(100, alignment: Alignment.Top),
                    new VStack()
                    {
                        new TrainControllerPanel(trainControls),
                        new DrawableControl(controlDelegate)
                    }
                }.FillHorizontal();
            };

            _gameTimer = gameTimer;
            _gameTimer.Elapsed += (s, e) =>
            {
                game.AdjustViewPortIfNecessary();

                controlDelegate.FlagDraw();
                _miniMapDelegate.FlagDraw();

                ThreadHelper.Run(async () =>
                {
                    await ThreadHelper.SwitchToMainThreadAsync();

                    controlDelegate.Invalidate();
                    _miniMapDelegate.Invalidate();
                });
            };
            _game = game;
            _pixelMapper = pixelMapper;
            _trackParameters = trackParameters;
            _trackLayout = trackLayout;
            _gameStorage = gameStorage;

            void SwitchGameMode()
            {
                trainControls.ToggleBuildMode();

                if (controlDelegate == null) return;

                controlDelegate.CurrentTool.Value = tools.FirstOrDefault(t => ShouldShowTool(trainControls.BuildMode, t));
            }
        }

        public void Save()
        {
            _gameStorage.WriteTracks(_trackLayout);
        }

        private void Zoom(int delta)
        {
            int currentZoom = _trackParameters.CellSize / 20;

            int newZoom = currentZoom + delta;
            if(newZoom >= 1 && newZoom <= 12)
            {
                _trackParameters.CellSize = newZoom * 20;
                _game.FlushCache();
                _pixelMapper.AdjustViewPort(_trackParameters.CellSize * -delta, _trackParameters.CellSize * -delta);
            }
        }

        public void Redraw(Size newSize)
        {
            if (Math.Abs(newSize.Width - _lastSize.Width) >= 20 ||
                Math.Abs(newSize.Height - _lastSize.Height) >= 20)
            {
                _lastSize = newSize;
                ViewPropertyChanged(ResetPropertyString, null);
            }
        }

        private static View CreateCommandControls(IEnumerable<ICommand> commands)
        {
            var controlsGroup = new VStack();
            foreach (ICommand cmd in commands)
            {
                controlsGroup.Add(new Button(cmd.Name, () => cmd.Execute()));
            }

            return controlsGroup;
        }

        private static View CreateToolsControls(IEnumerable<ITool> tools, TrainsDelegate controlDelegate, bool buildMode)
        {
            var controlsGroup = new RadioGroup(Orientation.Vertical);
            foreach (ITool tool in tools)
            {
                if (ShouldShowTool(buildMode, tool))
                {
                    if (controlDelegate.CurrentTool.Value == null)
                    {
                        controlDelegate.CurrentTool.Value = tool;
                    }

                    controlsGroup.Add(new RadioButton(() => tool.Name, () => controlDelegate.CurrentTool.Value == tool, () => controlDelegate.CurrentTool.Value = tool));
                }
            }

            return controlsGroup;
        }

        private static bool ShouldShowTool(bool buildMode, ITool tool)
            => (buildMode, tool.Mode) switch
            {
                (true, ToolMode.Build) => true,
                (false, ToolMode.Play) => true,
                (_, ToolMode.All) => true,
                _ => false
            };

        private static View CreateConfigurationControls(IEnumerable<ILayerRenderer> layers)
        {
            var layersGroup = new VStack();
            foreach (ILayerRenderer layer in layers)
            {
                layersGroup.Add(new ToggleButton(layer.Name, layer.Enabled, () => layer.Enabled = !layer.Enabled));
            }
            return layersGroup;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _gameTimer.Dispose();
                _miniMapDelegate.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
