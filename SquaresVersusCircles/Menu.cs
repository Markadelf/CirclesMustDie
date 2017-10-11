using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquaresVersusCircles
{
    class Menu
    {
        //Resources
        public static SpriteFont Myfont { get; set; }
        public static SpriteFont Bigfont { get; set; }
        public static Texture2D ClearTexture { get; set; }
        public static Texture2D MenuTexture { get; set; }
        private Room r;
        public Rectangle Screen { get; set; }


        //Contructor links it to the room
        public Menu(Room r){
            this.r = r;
            Screen = new Rectangle(0, 0, 15 * 32, 10 * 32);
        }

        public void Draw(SpriteBatch sb)
        {
            //Draw all the menu stuff
            sb.Draw(MenuTexture, Screen, Color.Khaki);
            sb.Draw(Quad.SquareTexture, new Rectangle(Screen.X + Screen.Width / 5, Screen.Y + 64, Screen.Width*3/5, 228), Color.WhiteSmoke);
            sb.DrawString(Bigfont, "Circles Must Die!", new Vector2(Screen.X + Screen.Width / 5, Screen.Y + 20), Color.Black);
            sb.DrawString(Myfont, "Level: " + r.RoomNumber, new Vector2(Screen.X + Screen.Width / 5 + 4, Screen.Y + 68), Color.Black);
            sb.DrawString(Myfont, "Controls:\nP: Pause/Unpause\nQ: Quit\nArrows (Menu): Select Level\nR: Restart Level"+
                "\n\nCharacter Controls:\nArrows (In Game): Shoot\nA/D: Move Left/Right\nSpace/W: Jump"
                , new Vector2(Screen.X + Screen.Width / 5 + 4, Screen.Y + 96), Color.Black);
            sb.Draw(Quad.SquareTexture, new Rectangle(Screen.X + Screen.Width - 176, Screen.Y + Screen.Height - 16, 176, 16), Color.Beige);
            sb.DrawString(Myfont, "Made by: Mark Delfavero", new Vector2(Screen.X + Screen.Width - 176, Screen.Y + Screen.Height - 16), Color.Black);

        }
    }
}
