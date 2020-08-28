﻿using Trains.NET.Engine;

namespace Trains.NET.Rendering
{
    [Order(0)]
    public class TerrainRenderer : ICachableLayerRenderer
    {
        private bool _dirty;

        private readonly ITerrainMap _terrainMap;

        public TerrainRenderer(ITerrainMap terrainMap)
        {
            _terrainMap = terrainMap;

            _terrainMap.CollectionChanged += (s, e) => _dirty = true;
        }

        public bool Enabled { get; set; } = true;
        public string Name => "Terrain";
        public bool IsDirty => _dirty;

        public void Render(ICanvas canvas, int width, int height, IPixelMapper pixelMapper)
        {
            if (_terrainMap.IsEmpty())
            {
                canvas.DrawRect(0, 0, pixelMapper.ViewPortWidth, pixelMapper.ViewPortHeight, new PaintBrush { Style = PaintStyle.Fill, Color = TerrainColourLookup.DefaultColour });
                return;
            }

            // Draw any non-grass cells
            foreach (Terrain terrain in _terrainMap)
            {
                Color colour = TerrainColourLookup.GetTerrainColour(terrain);

                (int x, int y, bool onScreen) = pixelMapper.CoordsToViewPortPixels(terrain.Column, terrain.Row);

                if (!onScreen) continue;

                canvas.DrawRect(x, y, pixelMapper.CellSize, pixelMapper.CellSize, new PaintBrush { Style = PaintStyle.Fill, Color = colour });
            }
            
            _dirty = false;
        }
    }
}
