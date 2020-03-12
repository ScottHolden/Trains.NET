﻿using System;
using System.Linq;
using Trains.NET.Engine;
using Xunit;

#nullable disable

namespace Trains.NET.Tests.FullGameTests.MovementTest
{
    public class PointToPoint_SingleStep : PointToPoint
    {
        public PointToPoint_SingleStep() : base(1) { }
    }
    public class PointToPoint_2Step : PointToPoint
    {
        public PointToPoint_2Step() : base(2) { }
    }
    public class PointToPoint_3Step : PointToPoint
    {
        public PointToPoint_3Step() : base(3) { }
    }
    public class PointToPoint_10Step : PointToPoint
    {
        public PointToPoint_10Step() : base(10) { }
    }
    public abstract class PointToPoint
    {
        private readonly int _movementSteps;
        private const int MovementPrecision = 4;

        public PointToPoint(int movementSteps)
        {
            _movementSteps = movementSteps;
        }

        [Theory]
        [InlineData(1, 1, 90.0f, 1, 3)]
        [InlineData(1, 3, 270.0f, 1, 1)]
        public void MovementTest_PointToPoint_3VerticalTracks(int startingColumn, int startingRow, float angle, int expectedColumn, int expectedRow)
        {
            var board = new GameBoard(false);

            board.AddTrack(1, 1);
            board.AddTrack(1, 2);
            board.AddTrack(1, 3);

            board.AddTrain(startingColumn, startingRow);

            Train train = board.GetTrains().Single();
            train.Angle = angle;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(angle, train.Angle, MovementPrecision);

            // Traveling from middle of 1 straight + whole 2nd straight + to middle of 3rd straight
            float distance = 0.5f + 1.0f + 0.5f;

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                board.GameLoopStep(distance / _movementSteps);

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(angle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(1, 1, 0.0f, 3, 1)]
        [InlineData(3, 1, 180.0f, 1, 1)]
        public void MovementTest_PointToPoint_3HorizontalTracks(int startingColumn, int startingRow, float angle, int expectedColumn, int expectedRow)
        {
            var board = new GameBoard(false);

            board.AddTrack(1, 1);
            board.AddTrack(2, 1);
            board.AddTrack(3, 1);

            board.AddTrain(startingColumn, startingRow);

            Train train = board.GetTrains().Single();
            train.Angle = angle;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(angle, train.Angle, MovementPrecision);

            // Traveling from middle of 1 straight + whole 2nd straight + to middle of 3rd straight
            float distance = 0.5f + 1.0f + 0.5f;

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                board.GameLoopStep(distance / _movementSteps);

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(angle, train.Angle, MovementPrecision);
        }


        [Theory]
        [InlineData(1, 2, 270.0f, 2, 1, 0.0f)]
        [InlineData(2, 1, 180.0f, 1, 2, 90.0f)]
        public void MovementTest_PointToPoint_Vertical_RightDown_Horizontal(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            var board = new GameBoard(false);

            board.AddTrack(1, 2); // Vertical
            board.AddTrack(1, 1); // Corner
            board.AddTrack(2, 1); // Horizontal

            board.AddTrain(startingColumn, startingRow);

            Train train = board.GetTrains().Single();
            train.Angle = startingAngle;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Traveling from middle of 1 straight + curve + to middle of 2nd straight
            //  This assumes that track radius is 0.5f
            float radius = 0.5f;
            float circleCircumference = 2.0f * (float)Math.PI * radius;

            float distance = 1.0f + circleCircumference / 4.0f;

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                board.GameLoopStep(distance / _movementSteps);

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(1, 1, 90.0f, 2, 2, 0.0f)]
        [InlineData(2, 2, 180.0f, 1, 1, 270.0f)]
        public void MovementTest_PointToPoint_Horizontal_RightUp_Vertical(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            var board = new GameBoard(false);

            board.AddTrack(1, 1); // Vertical
            board.AddTrack(1, 2); // Corner
            board.AddTrack(2, 2); // Horizontal

            board.AddTrain(startingColumn, startingRow);

            Train train = board.GetTrains().Single();
            train.Angle = startingAngle;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Traveling from middle of 1 straight + curve + to middle of 2nd straight
            //  This assumes that track radius is 0.5f
            float radius = 0.5f;
            float circleCircumference = 2.0f * (float)Math.PI * radius;

            float distance = 1.0f + circleCircumference / 4.0f;

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                board.GameLoopStep(distance / _movementSteps);

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }


        [Theory]
        [InlineData(1, 1, 0.0f, 2, 2, 90.0)]
        [InlineData(2, 2, 270.0f, 1, 1, 180.0f)]
        public void MovementTest_PointToPoint_Horizontal_LeftDown_Vertical(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            var board = new GameBoard(false);

            board.AddTrack(1, 1); // Horizontal
            board.AddTrack(2, 1); // Corner
            board.AddTrack(2, 2); // Vertical

            board.AddTrain(startingColumn, startingRow);

            Train train = board.GetTrains().Single();
            train.Angle = startingAngle;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Traveling from middle of 1 straight + curve + to middle of 2nd straight
            //  This assumes that track radius is 0.5f
            float radius = 0.5f;
            float circleCircumference = 2.0f * (float)Math.PI * radius;

            float distance = 0.5f + circleCircumference / 4.0f + 0.5f;

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                board.GameLoopStep(distance / _movementSteps);

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(1, 2, 0.0f, 2, 1, 270.0f)]
        [InlineData(2, 1, 90.0f, 1, 2, 180.0f)]
        public void MovementTest_PointToPoint_Horizontal_LeftUp_Vertical(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            var board = new GameBoard(false);

            board.AddTrack(1, 2); // Horizontal
            board.AddTrack(2, 2); // Corner
            board.AddTrack(2, 1); // Vertical

            board.AddTrain(startingColumn, startingRow);

            Train train = board.GetTrains().Single();
            train.Angle = startingAngle;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Traveling from middle of 1 straight + curve + to middle of 2nd straight
            //  This assumes that track radius is 0.5f
            //float distance = 0.5f + (float)Math.PI / 4.0f + 0.5f;

            float radius = 0.5f;
            float circleCircumference = 2.0f * (float)Math.PI * radius;

            float distance = 0.5f + circleCircumference / 4.0f + 0.5f;

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                board.GameLoopStep(distance / _movementSteps);

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }
    }
}
