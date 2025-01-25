﻿using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Xna = Microsoft.Xna.Framework;
using Pixxl.Materials;
using MonoGame.Extended;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;

namespace Pixxl.Gui
{
    public class Input : Widget
    {
        public SpriteBatch Batch { get; private set; }
        public Xna.Vector2 Dimensions { get; private set; }
        public string Text { get; set; }
        public Rectangle Rect
        {
            get { return new((int)Location.X, (int)Location.Y, (int)Dimensions.X, (int)Dimensions.Y); }
        }
        public Xna.Color Color { get; private set; }
        public Xna.Color Highlight { get; private set; }
        public SpriteFont Font { get; private set; }
        public Color Foreground { get; private set; }
        public Delegate? Function { get; private set; }
        public int Border { get; private set; }
        public Color BorderColor { get; private set; }
        public object?[]? Args { get; private set; }
        public bool Selected { get; private set; }
        public int Cursor { get; private set; }
        public Keys[] Previous { get; private set; }
        private float blink { get; set; }
        private int cursorX { get; set; }
        // Centering
        private int Textsize { get; set; }
        private int Charsize { get; set; }
        private Dictionary<string, char> specialKeys = new()
        {
            ["OemPeriod"] = '.',
            ["OemComma"] = ',',
            ["OemQuestion"] = '/',
            ["OemSemicolon"] = ';',
            ["OemQuotes"] = '\'',
            ["OemPlus"] = '=',
            ["OemMinus"] = '-',
            ["OemPipe"] = '\\',
            ["OemOpenBrackets"] = '[',
            ["OemCloseBrackets"] = ']',
            ["OemTilde"] = '`',
            ["Space"] = ' ',
            ["D1"] = '1',
            ["D2"] = '2',
            ["D3"] = '3',
            ["D4"] = '4',
            ["D5"] = '5',
            ["D6"] = '6',
            ["D7"] = '7',
            ["D8"] = '8',
            ["D9"] = '9',
            ["D0"] = '0',
        };
        private Dictionary<string, char> upperSymbols = new()
        {
            ["D1"] = '!',
            ["D2"] = '@',
            ["D3"] = '#',
            ["D4"] = '$',
            ["D5"] = '%',
            ["D6"] = '^',
            ["D7"] = '&',
            ["D8"] = '*',
            ["D9"] = '(',
            ["D0"] = ')',
            ["OemPeriod"] = '>',
            ["OemComma"] = '<',
            ["OemQuestion"] = '?',
            ["OemSemicolon"] = ':',
            ["OemQuotes"] = '"',
            ["OemPlus"] = '+',
            ["OemMinus"] = '_',
            ["OemPipe"] = '|',
            ["OemOpenBrackets"] = '{',
            ["OemCloseBrackets"] = '}',
            ["OemTilde"] = '~',
        };
        private string[] controlKeys = [ "Back", "Left", "Right" ];
        public Input(SpriteBatch batch, Xna.Vector2 location, Xna.Vector2 dimensions, Color foreground, Xna.Color color, Xna.Color highlight, SpriteFont font, int border = 3, Color borderColor = default)
        {
            Batch = batch;
            Location = location;
            Dimensions = dimensions;
            Text = "";
            Textsize = 0;
            blink = 0;
            cursorX = 0;
            Charsize = (int)font.MeasureString("_").X;
            Font = font;
            Foreground = foreground;
            Color = color;
            Highlight = highlight;
            Border = border;
            BorderColor = (borderColor == default ? Color.Black : borderColor);
            Selected = false;
            Cursor = 0;
            Previous = [];
        }
        public override void Update(Window window)
        {
            // Blink
            if (Selected) { blink = (blink + window.Delta) % 1.4f; }
            else blink = .7f;

            // Clicking
            MouseState mouseState = window.mouse;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (PointRectCollide(Location, Dimensions, mouseState.Position)) { Selected = true; }
                else { Selected = false; }
            }

            // Typing
            if (!Selected) { return; }
            bool shifted = window.keys.Contains(Keys.LeftShift) || window.keys.Contains(Keys.RightShift);
            char specialKeyname, specialUpperKeyname;
            foreach (Keys key in window.keys)
            {
                string keyname = key.ToString();
                if (!Previous.Contains(key) && (Textsize + Charsize < Dimensions.X - 2 * Border || controlKeys.Contains(keyname)))
                {
                    // Uppercase letter
                    if (keyname.Length == 1 && shifted) { Text = Text.Insert(Cursor, keyname); }
                    // Lowercase letter
                    else if (keyname.Length == 1) { Text = Text.Insert(Cursor, keyname.ToLower()); }
                    // Lowercase symbol
                    else if (!shifted && specialKeys.TryGetValue(keyname, out specialKeyname)) { Text = Text.Insert(Cursor, specialKeyname.ToString()); ; }
                    // Uppercase symbol
                    else if (shifted && specialKeys.ContainsKey(keyname) && upperSymbols.TryGetValue(keyname, out specialUpperKeyname)) { Text = Text.Insert(Cursor, specialUpperKeyname.ToString()); }
                    // Backspace
                    else if (keyname == "Back" && Text.Length > 0) { Text = Text.Remove(Cursor - 1, 1); }
                    // Move cursor right
                    else if (keyname == "Right" && Cursor < Text.Length) { Cursor++; }
                    // Move cursor left
                    else if (keyname == "Left" && Cursor > 0) { Cursor--; }
                    // Continue
                    else { continue; }
                    // Every taken key other than backspace, left arrow, and right arrow adds a char
                    if (keyname == "Back") { Cursor--; }
                    else if (!controlKeys.Contains(keyname)) { Cursor++; }
                    Recalculate();
                }
            }
            Previous = window.keys;
        }
        public override void Draw()
        {
            // Not drawing
            if (!Visible) { return; }

            // Background
            Batch.FillRectangle(Rect, Selected ? Highlight : Color);
            // Outline
            Batch.DrawRectangle(Rect, BorderColor);

            // Text
            if (Font != null)
            {
                Batch.DrawString(Font, Text, new(Location.X + Border, Location.Y + Border), Foreground);
            } else if (Text != "")
            {
                Logger.Log($"Skipping drawing text '{Text}' because of uninitialized font");
            }

            // Cursor
            if (blink >= .7) { Batch.DrawLine(Location.X + Border + cursorX, Location.Y + Border + 2, Location.X + Border + cursorX, Location.Y + Dimensions.Y - 4, Color.Black, 2); }
        }
        public void Recalculate()
        {
            // Recalculate text size
            Textsize = Text.Length > 0 ? (int)Font.MeasureString(Text).X : 0;
            cursorX = (int)Font.MeasureString(Text[..Cursor]).X;
            blink = .71f;
        }

        // Static
        public int TextMeasure(SpriteFont font, char character) { return (int)font.MeasureString(character.ToString()).X; }
        public int TextMeasure(SpriteFont font, string character) { return (int)font.MeasureString(character).X; }
    }
}
