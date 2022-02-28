// By Brevin Bell, and Charles Li

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game_Controller;
using TankWars;


namespace View
{
    /// <summary>
    /// view for TankWars
    /// </summary>
    public partial class Form1 : Form
    {
        private Controller controller;
        private bool isConnected;
        private HashSet<string> keysDown;
        private Image RedTank;
        private Image YellowTank;
        private Image GreenTank;
        private Image DarkTank;
        private Image LightGreenTank;
        private Image OrangeTank;
        private Image PurpleTank;
        private Image BlueTank;
        private Image WallImage;
        private Image RedTurret;
        private Image YellowTurret;
        private Image GreenTurret;
        private Image DarkTurret;
        private Image LightGreenTurret;
        private Image OrangeTurret;
        private Image PurpleTurret;
        private Image BlueTurret;
        private Image shot_yellow;
        private Image shot_red;
        private Image shot_green;
        private Image shot_grey;
        private Image shot_blue;
        private Image shot_violet;
        private Image worldImage;
        private Dictionary<int, BeamAnimation> animationBeams;
        private Dictionary<int, TankDeathAnimation> tdeaths;



        public Form1()
        {
            InitializeComponent();
            controller = new Controller();
            controller.Connected += connected;
            controller.Error += error;
            controller.UpdateArrived += OnFrame;
            isConnected = false;
            DoubleBuffered = true;
            keysDown = new HashSet<string>();
            animationBeams = new Dictionary<int, BeamAnimation>();
            tdeaths = new Dictionary<int, TankDeathAnimation>();
            //load images once to prevent slow run times
            RedTank = Image.FromFile("..\\..\\..\\Resources\\Images\\RedTank.png");
            YellowTank = Image.FromFile("..\\..\\..\\Resources\\Images\\YellowTank.png");
            GreenTank = Image.FromFile("..\\..\\..\\Resources\\Images\\GreenTank.png");
            DarkTank = Image.FromFile("..\\..\\..\\Resources\\Images\\DarkTank.png");
            LightGreenTank = Image.FromFile("..\\..\\..\\Resources\\Images\\LightGreenTank.png");
            OrangeTank = Image.FromFile("..\\..\\..\\Resources\\Images\\OrangeTank.png");
            PurpleTank = Image.FromFile("..\\..\\..\\Resources\\Images\\PurpleTank.png");
            BlueTank = Image.FromFile("..\\..\\..\\Resources\\Images\\BlueTank.png");
            WallImage = Image.FromFile("..\\..\\..\\Resources\\Images\\WallSprite.png");
            RedTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\RedTurret.png");
            YellowTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\YellowTurret.png");
            GreenTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\GreenTurret.png");
            DarkTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\DarkTurret.png");
            LightGreenTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\LightGreenTurret.png");
            OrangeTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\OrangeTurret.png");
            PurpleTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\PurpleTurret.png");
            BlueTurret = Image.FromFile("..\\..\\..\\Resources\\Images\\BlueTurret.png");
            shot_blue = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-blue.png");
            shot_violet = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-violet.png");
            shot_grey = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-grey.png");
            shot_green = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-green.png");
            shot_red = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-red.png");
            shot_yellow = Image.FromFile("..\\..\\..\\Resources\\Images\\shot-yellow.png");
            worldImage = Image.FromFile("..\\..\\..\\Resources\\Images\\Background.png");

        }

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// run when Connected event from controller happens
        /// </summary>
        private void connected()
        {
            isConnected = true;
            KeyPreview = true;
            controller.getWorld().TankDied += tDied;
        }

        /// <summary>
        /// run when Error event from controller happens
        /// </summary>
        private void error(string err)
        {
            MessageBox.Show(err);

        }

        /// <summary>
        /// run when a tank dies
        /// </summary>
        /// <param name="t"></param>
        private void tDied(Tank t)
        {
            //add animation object to dictionary of tank death animations
            TankDeathAnimation a = new TankDeathAnimation(t);
            lock ("world")
            {
                tdeaths[t.getId()] = a;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void serverTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// redraws form every frame
        /// </summary>
        private void OnFrame()
        {
            // Invalidate this form and all its children
            // This will cause the form to redraw as soon as it can
            MethodInvoker invalidator = new MethodInvoker(() => this.Invalidate(true));
            this.Invoke(invalidator);


        }

        /// <summary>
        /// sends current mouse location to controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (isConnected)
            {
                int mouseX = e.X - 450;
                int mouseY = e.Y - 450;
                controller.lookDir(mouseX, mouseY);
            }
            // Update the mouse path that is drawn onto the Panel.

        }

        /// <summary>
        /// if a mouse isnt clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control1_MouseUp(Object sender, MouseEventArgs e)
        {
            //a turret isnt firing
            if (isConnected)
            {
                controller.fireTurret("none");
            }

        }

        /// <summary>
        /// When a key is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            lock ("keys")
            {
                //add control related to key to a hashset
                if (isConnected)
                {
                    if (e.KeyCode == Keys.Escape)
                        Application.Exit();
                    if (e.KeyCode == Keys.W)
                    {
                        controller.moveUp();
                        keysDown.Add("up");
                    }
                    if (e.KeyCode == Keys.S)
                    {
                        controller.moveDown();
                        keysDown.Add("down");
                    }
                    if (e.KeyCode == Keys.A)
                    {
                        controller.moveLeft();
                        keysDown.Add("left");
                    }
                    if (e.KeyCode == Keys.D)
                    {
                        controller.moveRight();
                        keysDown.Add("right");
                    }
                }

                // Prevent other key handlers from running
                e.SuppressKeyPress = true;
                e.Handled = true;
            }

        }

        /// <summary>
        /// Key up handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            if (isConnected)
            {
                KeyUpHelper(e);
            }
        }

        /// <summary>
        /// sends key information to controller 
        /// </summary>
        /// <param name="e"></param>
        private void KeyUpHelper(KeyEventArgs e)
        {
            lock ("keys")
            {
                //remove information from hashset when a key is released
                if (e.KeyCode == Keys.W)
                {
                    keysDown.Remove("up");
                }
                if (e.KeyCode == Keys.S)
                {
                    keysDown.Remove("down");
                }
                if (e.KeyCode == Keys.A)
                {
                    keysDown.Remove("left");
                }
                if (e.KeyCode == Keys.D)
                {
                    keysDown.Remove("right");
                }

                if (keysDown.Count == 0)
                {
                    controller.noMove();
                    return;
                }
                //send information to controller based on what the first item in the hashset is
                string movDir = keysDown.First();
                if (movDir == "up")
                {
                    controller.moveUp();
                }
                else if (movDir == "down")
                {
                    controller.moveDown();
                }
                else if (movDir == "left")
                {
                    controller.moveLeft();
                }
                else if (movDir == "right")
                {
                    controller.moveRight();
                }
            }
        }

        /// <summary>
        /// When connect button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectButton_Click(object sender, EventArgs e)
        {
            //disable text boxes
            serverTextBox.Enabled = false;
            connectButton.Enabled = false;
            nameTextBox.Enabled = false;
            //send information in textboxes to controller for starting a server
            controller.StartConnection(serverTextBox.Text, nameTextBox.Text);
        }

        /// <summary>
        /// helper for drawing objects in the form
        /// </summary>
        /// <param name="e"></param>
        /// <param name="o"></param>
        /// <param name="worldX"></param>
        /// <param name="worldY"></param>
        /// <param name="angle"></param>
        /// <param name="drawer"></param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // "push" the current transform
            System.Drawing.Drawing2D.Matrix oldMatrix = e.Graphics.Transform.Clone();

            e.Graphics.TranslateTransform((int)worldX, (int)worldY);
            e.Graphics.RotateTransform((float)angle);
            drawer(o, e);

            // "pop" the transform
            e.Graphics.Transform = oldMatrix;
        }

        /// <summary>
        /// draws tanks
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void TankDrawer(object o, PaintEventArgs e)
        {
            string color = "";
            int tankWidth = 60;
            Tank t = o as Tank;
            //gives a color based on id
            if ((t.getId() % 8) == 0)
                color = "yellow";
            else if ((t.getId() % 8) == 1)
                color = "red";
            else if ((t.getId() % 8) == 2)
                color = "green";
            else if ((t.getId() % 8) == 3)
                color = "dark";
            else if ((t.getId() % 8) == 4)
                color = "lightGreen";
            else if ((t.getId() % 8) == 5)
                color = "orange";
            else if ((t.getId() % 8) == 6)
                color = "purple";
            else if ((t.getId() % 8) == 7)
                color = "blue";
            Rectangle r = new Rectangle(-(tankWidth / 2), -(tankWidth / 2), tankWidth, tankWidth);

            if (color == "red")
            {
                e.Graphics.DrawImage(RedTank, r);
            }
            else if (color == "yellow")
            {
                e.Graphics.DrawImage(YellowTank, r);
            }
            else if (color == "green")
            {
                e.Graphics.DrawImage(GreenTank, r);
            }
            else if (color == "dark")
            {
                e.Graphics.DrawImage(DarkTank, r);
            }
            else if (color == "lightGreen")
            {
                e.Graphics.DrawImage(LightGreenTank, r);
            }
            else if (color == "orange")
            {
                e.Graphics.DrawImage(OrangeTank, r);
            }
            else if (color == "purple")
            {
                e.Graphics.DrawImage(PurpleTank, r);
            }
            else if (color == "blue")
            {
                e.Graphics.DrawImage(BlueTank, r);
            }
        }

        /// <summary>
        /// draws powerups
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void PowerupDrawer(object o, PaintEventArgs e)
        {
            string color = "red";
            int width = 15;
            int width2 = 8;
            Powerup t = o as Powerup;

            Rectangle r = new Rectangle(-(width / 2), -(width / 2), width, width);
            Rectangle r2 = new Rectangle(-(width2 / 2), -(width2 / 2), width2, width2);
            using (System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
            using (System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green))
            {
                //square within a larger square
                if (color == "red")
                {
                    e.Graphics.FillRectangle(redBrush, r);
                    e.Graphics.FillRectangle(greenBrush, r2);
                }
            }
        }

        /// <summary>
        /// draws walls
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void WallDrawer(object o, PaintEventArgs e)
        {
            int width = 50;
            Wall t = o as Wall;

            Rectangle r = new Rectangle(-(width / 2), -(width / 2), width, width);

            e.Graphics.DrawImage(WallImage, r);
        }

        /// <summary>
        /// draws projectiles
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            int width = 30;
            Projectile p = o as Projectile;
            string color = "";
            Projectile t = o as Projectile;
            if ((t.getId() % 8) == 0)
                color = "yellow";
            else if ((t.getId() % 8) == 1)
                color = "red";
            else if ((t.getId() % 8) == 2)
                color = "green";
            else if ((t.getId() % 8) == 3)
                color = "dark";
            else if ((t.getId() % 8) == 4)
                color = "lightGreen";
            else if ((t.getId() % 8) == 5)
                color = "orange";
            else if ((t.getId() % 8) == 6)
                color = "purple";
            else if ((t.getId() % 8) == 7)
                color = "blue";

            Rectangle r = new Rectangle(-(width / 2), -(width / 2), width, width);
            using (System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
            {
                if (color == "yellow")
                {
                    e.Graphics.DrawImage(shot_yellow, r);
                }
                else if (color == "red")
                {

                    e.Graphics.DrawImage(shot_red, r);
                }
                else if (color == "green")
                {
                    e.Graphics.DrawImage(shot_green, r);
                }
                else if (color == "dark")
                {
                    e.Graphics.DrawImage(shot_grey, r);
                }
                else if (color == "lightGreen")
                {
                    e.Graphics.DrawImage(shot_green, r);
                }
                else if (color == "orange")
                {
                    e.Graphics.DrawImage(shot_yellow, r);
                }
                else if (color == "purple")
                {
                    e.Graphics.DrawImage(shot_violet, r);
                }
                else if (color == "blue")
                {
                    e.Graphics.DrawImage(shot_blue, r);
                }
            }
        }

        /// <summary>
        /// draws beams
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void BeamDrawer(object o, PaintEventArgs e)
        {
            string color = "red";
            Beam t = o as Beam;
            BeamAnimation animation;
            //add beamanimation to dictionary if not already in it
            if (!(animationBeams.ContainsKey(t.getId())))
            {
                animation = new BeamAnimation(t.getId(), t);
                animationBeams[t.getId()] = animation;
            }
            animation = animationBeams[t.getId()];
            //width will change on everyframe we draw it making our beam animation
            int width = animation.getFrame();

            Rectangle r = new Rectangle(-(width / 2), -(width / 2), width, 5000);
            using (System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))

            {
                if (color == "red")
                {
                    e.Graphics.FillRectangle(redBrush, r);
                }
            }
        }

        /// <summary>
        /// draws healthbars
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void HealthDrawer(object o, PaintEventArgs e)
        {
            int height = 5;
            int width = 45;
            Tank t = o as Tank;
            //draws a green, yellow or red healthbar based on if a tanks hp is 3,2 or 1
            if (t.getHP() == 3)
            {
                using (System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green))
                {
                    Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);
                    e.Graphics.FillRectangle(greenBrush, r);
                }
            }
            else if (t.getHP() == 2)
            {
                using (System.Drawing.SolidBrush yellowBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow))
                {
                    Rectangle r = new Rectangle(-(width / 2), -(height / 2), (width * 2) / 3, height);
                    e.Graphics.FillRectangle(yellowBrush, r);
                }
            }
            else if (t.getHP() == 1)
            {
                using (System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
                {
                    Rectangle r = new Rectangle(-(width / 2), -(height / 2), width / 3, height);
                    e.Graphics.FillRectangle(redBrush, r);
                }
            }
        }
        /// <summary>
        /// draws turrets
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void TurretDrawer(object o, PaintEventArgs e)
        {
            string color = "";
            int turretWidth = 50;
            Tank t = o as Tank;
            if ((t.getId() % 8) == 0)
                color = "yellow";
            else if ((t.getId() % 8) == 1)
                color = "red";
            else if ((t.getId() % 8) == 2)
                color = "green";
            else if ((t.getId() % 8) == 3)
                color = "dark";
            else if ((t.getId() % 8) == 4)
                color = "lightGreen";
            else if ((t.getId() % 8) == 5)
                color = "orange";
            else if ((t.getId() % 8) == 6)
                color = "purple";
            else if ((t.getId() % 8) == 7)
                color = "blue";
            Rectangle r = new Rectangle(-(turretWidth / 2), -(turretWidth / 2), turretWidth, turretWidth);

            if (color == "red")
            {
                e.Graphics.DrawImage(RedTurret, r);
            }
            else if (color == "yellow")
            {
                e.Graphics.DrawImage(YellowTurret, r);
            }
            else if (color == "green")
            {
                e.Graphics.DrawImage(GreenTurret, r);
            }
            else if (color == "dark")
            {
                e.Graphics.DrawImage(DarkTurret, r);
            }
            else if (color == "lightGreen")
            {
                e.Graphics.DrawImage(LightGreenTurret, r);
            }
            else if (color == "orange")
            {
                e.Graphics.DrawImage(OrangeTurret, r);
            }
            else if (color == "purple")
            {
                e.Graphics.DrawImage(PurpleTurret, r);
            }
            else if (color == "blue")
            {
                e.Graphics.DrawImage(BlueTurret, r);
            }
        }

        /// <summary>
        /// draws tank death animations
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void TankDeathDrawer(object o, PaintEventArgs e)
        {

            Tank t = o as Tank;
            TankDeathAnimation animation = tdeaths[t.getId()];
            // make it alternate between showing a rectangle and not
            if (animation.getFrame() == 0)
            {
                return;
            }
            if (animation.getFrame() % 16 == 0)
            {
                return;
            }
            if (animation.getFrame() % 16 == 1)
            {
                return;
            }
            if (animation.getFrame() % 16 == 3)
            {
                return;
            }
            if (animation.getFrame() % 16 == 4)
            {
                return;
            }
            int explostionwitdth = 45;
            Rectangle r = new Rectangle(-(explostionwitdth / 2) - (animation.getFrame() * 10) + 60 + (animation.getFrame() * 10) - 60, -(explostionwitdth / 2), explostionwitdth, explostionwitdth);
            using (System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
            {
                e.Graphics.FillRectangle(redBrush, r);
            }
        }

        /// <summary>
        /// draws player name and score 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ScoreDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 12);
            using (System.Drawing.SolidBrush blackBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black))
                e.Graphics.DrawString(t.getName() + ": " + t.getScore().ToString(), drawFont, blackBrush, -32, 0);
        }



        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!isConnected)
            {
                return;
            }
            World theWorld = controller.getWorld();
            // Center the view on the middle of the world,
            // since the image and world use different coordinate systems
            int worldSize = theWorld.getSize();
            int viewSize = 900; // view is square, so we can just use width
            Dictionary<int, Tank> tanks = theWorld.getTanks();
            if (!tanks.ContainsKey(controller.getID()))
            {
                return;
            }
            //centers the players view
            double playerX = tanks[controller.getID()].getLocation().GetX();
            double playerY = tanks[controller.getID()].getLocation().GetY();
            Rectangle r = new Rectangle((-worldSize / 2), (-worldSize / 2), worldSize, worldSize);



            e.Graphics.TranslateTransform((float)(-playerX + (viewSize / 2)), (float)(-playerY + (viewSize / 2)));
            e.Graphics.DrawImage(worldImage, r);

            lock ("world")
            {
                // Draw the tanks
                foreach (Tank t in theWorld.getTanks().Values)
                {
                    DrawObjectWithTransform(e, t, t.getLocation().GetX(), t.getLocation().GetY(), t.getOrientationB().ToAngle(), TankDrawer);
                    DrawObjectWithTransform(e, t, t.getLocation().GetX(), t.getLocation().GetY(), t.getOrientation().ToAngle(), TurretDrawer);
                    DrawObjectWithTransform(e, t, t.getLocation().GetX(), t.getLocation().GetY() - 40, 0, HealthDrawer);
                    DrawObjectWithTransform(e, t, t.getLocation().GetX(), t.getLocation().GetY() + 30, 0, ScoreDrawer);
                }
                // Draw the Walls
                foreach (Wall w in theWorld.getWalls().Values)
                {
                    foreach (Vector2D segment in w.wallSegments())
                    {
                        DrawObjectWithTransform(e, segment, segment.GetX(), segment.GetY(), 0, WallDrawer);
                    }

                }
                // Draw the powerups
                foreach (Powerup pow in theWorld.getPowerups().Values)
                {
                    DrawObjectWithTransform(e, pow, pow.getLocation().GetX(), pow.getLocation().GetY(), 0, PowerupDrawer);
                }

                // Draw the Projectiles
                foreach (Projectile p in theWorld.getProjectile().Values)
                {
                    DrawObjectWithTransform(e, p, p.getLocation().GetX(), p.getLocation().GetY(), p.getOrientation().ToAngle(), ProjectileDrawer);
                }
                // Draw the Beams
                foreach (Beam b in theWorld.getBeams().Values)
                {
                    DrawObjectWithTransform(e, b, b.getLocation().GetX(), b.getLocation().GetY(), (b.getOrientation().ToAngle() + 180), BeamDrawer);
                }
                foreach (BeamAnimation a in animationBeams.Values)
                {
                    Beam b = a.getbeam();
                    a.increment();
                    DrawObjectWithTransform(e, b, b.getLocation().GetX(), b.getLocation().GetY(), (b.getOrientation().ToAngle() + 180), BeamDrawer);
                }
                //draws tank animations
                foreach (TankDeathAnimation a in tdeaths.Values)
                {
                    Tank t = a.getTank();
                    a.increment();
                    DrawObjectWithTransform(e, t, t.getLocation().GetX(), t.getLocation().GetY(), (t.getOrientation().ToAngle() + 180), TankDeathDrawer);
                }
            }
            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }

        /// <summary>
        /// when mouse is clicked down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //send weapon information to controller
            if (isConnected)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    controller.fireTurret("alt");
                }
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    controller.fireTurret("main");
                }
            }
        }
    }
}

