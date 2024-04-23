﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tilt_Game
{
    public class Board
    {

        char[][] state;

        public Board(char[][] state)
        {
            this.state = state;
        }

        
        public Board(String state) { 
            //Convert String to 2d array
        }
        //Modify array when tilting the board in different directions
        #region Tilting Logic
        public Board TiltRight() {
            char[][] NewState = (char[][]) state.Clone();
            return new Board(NewState);
        }
        public Board TilitDown() {
            char[][] NewState = (char[][]) state.Clone();
            return new Board(NewState);
        }
        public Board TiltUp() {
            char[][] NewState = (char[][]) state.Clone();
            return new Board(NewState);
        }
        public Board TiltLeft() {
            char[][] NewState = (char[][]) state.Clone();
            return new Board(NewState);
        }
        #endregion
       
        public String toString() 
        {
            //Returns String representation of board
            return null;
        }

        #region Ignore
        public override bool Equals(object? obj)
        {
            return this.toString().Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.toString().GetHashCode();
        }
        #endregion
    }

    public class Player 
    {
        List<Board> Sequence = new List<Board>();
        int TargetX;
        int TargetY;
        public List<Board> Solve(char[][] initialBoard, int targetX, int targetY)
        {
            //Generate Sequence starting with initialBoard
            return Sequence;
        }

        public Boolean IsWinning(Board board) {
            //Check if the current configuration is winning
            return true;
        }
    }

    public class Cameraman 
    { 
        //public GUIComponent Representation(Board board)
        //{
        //Generate a grid GUI Component representing the current board configuration
        //}
    }
    
}