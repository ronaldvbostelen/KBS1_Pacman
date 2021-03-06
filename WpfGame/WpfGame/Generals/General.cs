﻿using System;
using WpfGame.Models;

namespace WpfGame.Generals
{
    public enum GameState
    {
        Playing,
        Finished,
        Lost,
        OutOfTime
    }

    public enum ObjectType
    {
        Coin,
        Obstacle,
        Wall,
        Path,
        Enemy,
        Player,
        SpawnPoint,
        StartPoint,
        EndPoint
    }

    public enum Move
    {
        Stop,
        Left,
        Right,
        Up,
        Down
    }

    public enum PacmanFacing
    {
        Left,
        LeftOpen,
        Right,
        RightOpen,
        Up,
        UpOpen,
        Down,
        DownOpen
    }

    public enum EnemyFacing
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum SelectedItem
    {
        None,
        Wall,
        Coin,
        Obstacle,
        Start,
        End,
        Erase,
        Spawn
    }

    public enum Collision
    {
        Clear,
        Wall,
        Border,
        Coin,
        Obstacle,
        Endpoint,
        Player,
        Enemy
    }

    public static class General
    {
        public const string PlaygroundPath = @"\Playgrounds\";
    }
}