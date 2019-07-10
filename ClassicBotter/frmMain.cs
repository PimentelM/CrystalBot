using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using CrystalBot.Objects;
using System.Media;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;

using System.Management;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Reflection;




namespace CrystalBot
{
    public partial class frmMain : Form
    {
        public int scriptIndex = 0;

        public Point savSelf;

        public uint TryAttackID = 123;
        public int TryAttkTryes = 0;
        public List<uint> TemporaryBlackList = new List<uint>();

        public string[] ignorelist = "NomeImpossivel1,23".Split(',');//= File.ReadAllLines("ignoremonster.txt");

        public List<uint> cavebotBlacklist = new List<uint>();

        public Dictionary<string, int> scriptVars = new Dictionary<string, int>(); 

        public int outHouseX = 0;   // Housing Variables out house
        public int outHouseY = 0;
        public int outHouseZ = 0;

        public int inHouseX = 0;   // Housing Variables in house
        public int inHouseY = 0;
        public int inHouseZ = 0;

        public string CavebotDebugInfo = "";


        Player player;

        Thread threadScript;
        Thread threadMain;
        Thread threadCavebot;
        Thread threadCast;

        SoundPlayer soundAlert;
        public static Hashtable ht;
        List<TextBox> hotkeyKeys;
        List<TextBox> hotkeyActions;
        List<uint> foods;
        List<uint> lootbags;
        //List<uint> loot;

        int spyFloor = 0;

        bool scriptWorking;
       




        /// <Console>
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();
        /// </Console>


        string HWID;



        public frmMain()
        {
            HWID = "Open Source";
            
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            InitKeyboard();
            InitMouse();

            LoadMain();

            Addresses.Version.SetAddresses();

            soundAlert = new SoundPlayer(Application.StartupPath + "/alert.wav");

            cmbxHousingStepDirection.SelectedItem = "W";
            cmbxAntiIdleDirection.SelectedItem = "S";
            cmbxExoriN.SelectedItem = "2";
            ht = new Hashtable();

            hotkeyKeys = new List<TextBox>();
            hotkeyActions = new List<TextBox>();
            InitHotkeys();


            Client.SetLight(true);
            //Client.NameSpyOn();


            LoadCavebotScripts();
            LoadScripts();
            LoadTags();

            foods = new List<uint>();
            try
            {
                using (StreamReader sr = new StreamReader("foods.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        foods.Add(uint.Parse(line));
                    }
                }
            }
            catch
            {

                foods.Add(2671);
                foods.Add(2666);
                foods.Add(3725);
                foods.Add(3577);
                foods.Add(3582);

            }

            lootbags = new List<uint>();
            lootbags.Add(2854);
            lootbags.Add(1987);






            var lib = new AutoCompleteStringCollection();
            lib.Add("$lclick");
            lib.Add("$rclick");
            lib.Add("$drag");
            lib.Add("$dragsmooth");
            lib.Add("$wait");
            lib.Add("$key");
            lib.Add("$gotoline");
            lib.Add("$gotolabel");
            lib.Add("$takeloot");
            lib.Add("$goto");
            lib.Add("$waypoint");
            lib.Add("$node");
            lib.Add("$ladder");
            lib.Add("$UseWpt");
            lib.Add("$pausecavebot");
            lib.Add("$resumecavebot");
            lib.Add("$pauseattacker");
            lib.Add("$resumeattacker");
            lib.Add("$pausescript");
            lib.Add("$resumescript");
            lib.Add("$playsound");
            lib.Add("$stackitems");
            txtCavebotAction.AutoCompleteCustomSource = lib;
            txtCavebotAction.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtCavebotAction.AutoCompleteSource = AutoCompleteSource.CustomSource;

            txtAction.AutoCompleteCustomSource = lib;
            txtAction.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtAction.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var libCE = new AutoCompleteStringCollection();
            libCE.Add("$hp");
            libCE.Add("$mp");
            libCE.Add("$cap");
            libCE.Add("$ring");
            libCE.Add("$level");
            libCE.Add("$mlevel");
            libCE.Add("$exp");
            libCE.Add("$manashield");
            libCE.Add("$haste");
            libCE.Add("$paralyze");
            libCE.Add("$neck");
            libCE.Add("$rhand");
            libCE.Add("$lhand");
            libCE.Add("$ring");
            libCE.Add("$ammo");
            libCE.Add("$playeronscreen");
            libCE.Add("$posx");
            libCE.Add("$posy");
            libCE.Add("$target.hppc");
            libCE.Add("$target.posx");
            libCE.Add("$target.posy");
            libCE.Add("$target.posz");
            libCE.Add("$mppc");

            txtThing1.AutoCompleteCustomSource = libCE;
            txtThing1.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtThing1.AutoCompleteSource = AutoCompleteSource.CustomSource;

        }

        private void LoadCavebotScripts()
        {
            lstCavebotScripts.Items.Clear();
            DirectoryInfo dInfo = new DirectoryInfo(Application.StartupPath + "/waypoints/");
            FileInfo[] files = dInfo.GetFiles("*.txt");
            foreach (FileInfo f in files)
            {
                string str = f.Name.Substring(0, f.Name.Length - 4);
                lstCavebotScripts.Items.Add(str);
            }
        }

        private void LoadScripts()
        {
            lstScripts.Items.Clear();
            DirectoryInfo dInfo = new DirectoryInfo(Application.StartupPath + "/scripts/");
            FileInfo[] files = dInfo.GetFiles("*.txt");
            foreach (FileInfo f in files)
            {
                string str = f.Name.Substring(0, f.Name.Length - 4);
                lstScripts.Items.Add(str);
            }
        }

        private void InitKeyboard()
        {
            KeyboardHook.Enable();
            KeyboardHook.Add(Keys.Pause, StopAll);
            KeyboardHook.Add(Keys.Insert, Recorder);
            KeyboardHook.Add(Keys.Add, SpyUp);
            KeyboardHook.Add(Keys.Subtract, SpyDown);
            KeyboardHook.Add(Keys.Home, RefreshTags);
        }

        private void InitTags()
        {
            if (chkAutoSelf.Checked)
            {
                WinApi.RECT rect = new WinApi.RECT();
                WinApi.GetWindowRect(Memory.process.MainWindowHandle, out rect);
                Rectangle gameview = Client.GameView();
                gameview.Y += rect.top + 35;
                gameview.X += rect.left + 10;
                Point p1 = new Point(gameview.X, gameview.Y);
                Point p2 = new Point(gameview.X + gameview.Width, gameview.Y + gameview.Height);


                if (ht.ContainsKey("#topleft")) ht.Remove("#topleft");
                if (ht.ContainsKey("#bottomright")) ht.Remove("#bottomright");
                if (ht.ContainsKey("#self")) ht.Remove("#self");

                ht.Add("#topleft", p1);
                ht.Add("#bottomright", p2);
                ht.Add("#self", new Point(gameview.Width / 2 + gameview.X, gameview.Height / 2 + gameview.Y));
            }
        }
        private bool SpyUp()
        {
            if (Memory.Ver == 3) return false;
            Client.StatusbarMessage = "Spying up";
            LevelSpy(++spyFloor);
            //if (spyFloor >= 6) spyFloor = 6;

            return false;
        }

        private bool SpyDown()
        {
            if (Memory.Ver == 3) return false;
            Client.StatusbarMessage = "Spying down";
            LevelSpy(--spyFloor);
            //if (spyFloor <= -6) spyFloor = -6;

            return false;
        }

        private bool LevelSpy(int floor)
        {
            int playerZ;
            int tempPtr;

            if (spyFloor == 0)
            {
                Memory.WriteBytes(Addresses.Client.LevelSpy1, Addresses.Client.LevelSpyDefault, 6);
                Memory.WriteBytes(Addresses.Client.LevelSpy2, Addresses.Client.LevelSpyDefault, 6);
                Memory.WriteBytes(Addresses.Client.LevelSpy3, Addresses.Client.LevelSpyDefault, 6);
                Client.StatusbarMessage = "Groundfloor";
                return false;
            }

            Memory.WriteBytes(Addresses.Client.LevelSpy1, Addresses.Client.Nops, 6);
            Memory.WriteBytes(Addresses.Client.LevelSpy2, Addresses.Client.Nops, 6);
            Memory.WriteBytes(Addresses.Client.LevelSpy3, Addresses.Client.Nops, 6);

            tempPtr = Memory.ReadInt(Addresses.Client.LevelSpyPtr);
            tempPtr += 0x1C;// Addresses.Client.LevelSpyAdd1;
            tempPtr = Memory.ReadInt(tempPtr);
            tempPtr += 0x25D8; //(int)Addresses.Client.LevelSpyAdd2;

            playerZ = (int)player.Z;

            if (playerZ <= 7)
            {
                if (playerZ - floor >= 0 && playerZ - floor <= 7)
                {
                    playerZ = 7 - playerZ;
                    Memory.WriteInt(tempPtr, playerZ + floor);
                    Debug.WriteLine(playerZ + floor);
                    return true;
                }
            }
            else
            {
                if (floor >= -2 && floor <= 2 && playerZ - floor < 16)
                {
                    Memory.WriteInt(tempPtr, 2 + floor);
                    return true;
                }
            }

            return false;
        }

        bool mLeft;
        bool mRight;
        private void InitMouse()
        {
            MouseHook.Enable();
            MouseHook.ButtonUp += new MouseHook.MouseButtonHandler(delegate(MouseButtons btn)
            {
                if (mLeft && mRight && Memory.Ver!=3) Client.StatusbarMessage = player.LastRightClickId.ToString();
                if (btn == MouseButtons.Left)
                    mLeft = false;
                if (btn == MouseButtons.Right)
                    mRight = false;
                return true;
            });
            MouseHook.ButtonDown += new MouseHook.MouseButtonHandler(delegate(MouseButtons btn)
            {
                if (WinApi.GetForegroundWindow() == Memory.process.MainWindowHandle)
                {
                    if (btn == MouseButtons.Right)
                    {
                        mRight = true;
                    }
                    if (btn == MouseButtons.Left)
                    {
                        mLeft = true;
                    }
                }
                return true;
            });
        }

        private void SaveHotkeys()
        {

            TextWriter tw = new StreamWriter("hotkeys.txt");
            for (int i = 0; i < 12; i++)
            {
                tw.WriteLine(hotkeyActions[i].Text);
            }
            tw.Close();
        }

        private void LoadHotkeys()
        {
            string[] lines = System.IO.File.ReadAllLines("hotkeys.txt");
            int count = 0;
            foreach (string line in lines)
            {
                hotkeyActions[count].Text = line;
                count++;
            }
        }



        private void InitHotkeys()
        {
            string[] lines = System.IO.File.ReadAllLines("hotkeys.txt");
            int count = 0;
            for (int i = 0; i < 12; i++)
            {
                CheckBox chkHotkey = new CheckBox();
                chkHotkey.Name = "chkHotkey" + count.ToString();
                chkHotkey.CheckedChanged += new EventHandler(chkHotkey_CheckedChanged);

                TextBox txtHotkeyKey = new TextBox();
                txtHotkeyKey.Name = "txtHotkeyKey" + count.ToString();
                txtHotkeyKey.ReadOnly = true;
                txtHotkeyKey.KeyDown += new KeyEventHandler(txtHotkeyKey_KeyDown);

                TextBox txtHotkeyAction = new TextBox();
                txtHotkeyAction.Name = "txtHotkeyAction" + count.ToString();
                txtHotkeyAction.Text = "";
                txtHotkeyAction.Width = 500;

                if (i < lines.Length)
                {
                    txtHotkeyAction.Text = lines[i];
                }


                tableLayoutPanel1.Controls.Add(chkHotkey);
                tableLayoutPanel1.Controls.Add(txtHotkeyKey);
                tableLayoutPanel1.Controls.Add(txtHotkeyAction);
                hotkeyKeys.Add(txtHotkeyKey);
                hotkeyActions.Add(txtHotkeyAction);
                count++;
                tableLayoutPanel1.RowCount += 1;
            }
        }

        void chkHotkey_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            int num = Int32.Parse(cb.Name.Substring(cb.Name.Length - 1, 1));
            byte val = 0;
            try
            {
                val = Byte.Parse(hotkeyKeys[num].Text);
            }
            catch
            {
                return;
            }
            Keys k = (Keys)val;
            if (cb.Checked)
            {
                try
                {
                    string line = hotkeyActions[num].Text;
                    KeyboardHook.Add(k, new KeyboardHook.KeyPressed(delegate()
                    {
                        if (line.Contains("$shot #"))
                        {
                            if (Memory.LicenseType == 3)
                            {
                                Client.StatusbarMessage = "You cannot use AimBot with FreeTrial License.";
                                return false;
                            }
                            Point rune = GetPoint(line.Split(' ')[1]);
                            Utils.MakeRightClick(rune.X, rune.Y);
                            Thread.Sleep(50);
                            int targID;
                            if (chkSaveLast.Checked) targID = LastTarget;
                            else targID = player.TargetId;

                            foreach (Creature c in new Battlelist().GetCreatures())
                            {
                                if (targID == c.Id)
                                {
                                    int PredictionX = 0;
                                    int PredictionY = 0;

                                    //if(c.isWalk==1 && player.isWalk ==0)
                                    //{
                                    //    switch (c.Direction)
                                    //    {
                                    //        case 0:
                                    //            PredictionY -= 1;
                                    //            break;
                                    //        case 2:
                                    //            PredictionY += 1;
                                    //            break;
                                    //        case 1:
                                    //            PredictionX += 1;
                                    //            break;
                                    //        case 3:
                                    //            PredictionX -= 1;
                                    //            break;


                                    //        case 4:
                                    //            PredictionX += 1;
                                    //            PredictionY += -1;
                                    //            break;

                                    //        case 5:
                                    //            PredictionX += 1;
                                    //            PredictionY += 1;
                                    //            break;

                                    //        case 6:
                                    //            PredictionX += -1;
                                    //            PredictionY += 1;
                                    //            break;

                                    //        case 7:
                                    //            PredictionX += -1;
                                    //            PredictionY += -1;
                                    //            break; 

                                    //    }


                                    //}
                                    Point TargPos = GetMapPoint(c.X + PredictionX, c.Y + PredictionY);
                                    Utils.MakeLeftClick(TargPos.X, TargPos.Y);
                                }
                            }



                            return false;
                        }
                        else if (line.Contains("$shotMw #"))
                        {
                            Point rune = GetPoint(line.Split(' ')[1]);
                            Utils.MakeRightClick(rune.X, rune.Y);
                            Thread.Sleep(50);
                            int targID;
                            if (chkSaveLast.Checked) targID = LastTarget;
                            else targID = player.TargetId;

                            foreach (Creature c in new Battlelist().GetCreatures())
                            {
                                if (targID == c.Id)
                                {
                                    int PredictionX = 0;
                                    int PredictionY = 0;

                                    if (c.isWalk == 1)
                                    {
                                        switch (c.Direction)
                                        {
                                            case 0:
                                                PredictionY -= 2;
                                                break;
                                            case 2:
                                                PredictionY += 2;
                                                break;
                                            case 1:
                                                PredictionX += 2;
                                                break;
                                            case 3:
                                                PredictionX -= 2;
                                                break;


                                            case 4:
                                                PredictionX += 2;
                                                PredictionY += -2;
                                                break;

                                            case 5:
                                                PredictionX += 2;
                                                PredictionY += 2;
                                                break;

                                            case 6:
                                                PredictionX += -2;
                                                PredictionY += 2;
                                                break;

                                            case 7:
                                                PredictionX += -2;
                                                PredictionY += -2;
                                                break;

                                        }
                                    }
                                    else
                                    {
                                        switch (c.Direction)
                                        {
                                            case 0:
                                                PredictionY -= 1;
                                                break;
                                            case 2:
                                                PredictionY += 1;
                                                break;
                                            case 1:
                                                PredictionX += 1;
                                                break;
                                            case 3:
                                                PredictionX -= 1;
                                                break;


                                            case 4:
                                                PredictionX += 1;
                                                PredictionY += -1;
                                                break;

                                            case 5:
                                                PredictionX += 1;
                                                PredictionY += 1;
                                                break;

                                            case 6:
                                                PredictionX += -1;
                                                PredictionY += 1;
                                                break;

                                            case 7:
                                                PredictionX += -1;
                                                PredictionY += -1;
                                                break;

                                        }


                                    }
                                    Point TargPos = GetMapPoint(c.X + PredictionX, c.Y + PredictionY);
                                    Utils.MakeLeftClick(TargPos.X, TargPos.Y);
                                }
                            }



                            return false;
                        }
                        else
                        {

                            EvaluateScript(line);
                            return false;
                        }
                    }));
                }
                catch { }
            }
            else
            {
                KeyboardHook.Remove(k);
            }

        }

        void txtHotkeyKey_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            Debug.WriteLine(tb.Name);
            tb.Text = e.KeyValue.ToString();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadPlayer();

            try
            {
                ignorelist = File.ReadAllLines("ignoremonster.txt");
            }
            catch { }

            try
            {
                string state = File.ReadAllText("autoget.txt");

                if (state == "0") chkAutoSelf.Checked = true;
                else chkAutoSelf.Checked = false;
            }
            catch { }


            threadScript = new Thread(Script);
            threadScript.Start();
            threadMain = new Thread(Main);
            threadMain.Start();
            threadCavebot = new Thread(Cavebot);
            threadCavebot.Start();
            threadCast = new Thread(Cast);
            threadCast.Start();
        }

        private bool Recorder()
        {
            RefreshTags();            
            if (true)
            {
                Form f = new Forms.frmRecorder();
                f.Show();
                return false;
            }
        }

        public bool RefreshTags()
        {
            LoadPlayer();
            WinApi.FlashWindow(Memory.process.MainWindowHandle, false);
            InitTags();
            lstTags.Items.Clear();
            foreach (string k in ht.Keys)
            {
                Point tmp;
                tmp = (Point)ht[k.ToString()];
                lstTags.Items.Add(k + " " + tmp.X + " " + tmp.Y);
            }
            return false;
        }

        private void SaveTags()
        {

            TextWriter tw = new StreamWriter("tags.txt");
            for (int i = 0; i < lstTags.Items.Count; i++)
            {
                tw.WriteLine(lstTags.Items[i].ToString());
            }
            tw.Close();
        }

        private void LoadTags()
        {
            ht.Clear();
            //load tags from txt to hashmap
            string line;
            TextReader tr = new StreamReader("tags.txt");
            while ((line = tr.ReadLine()) != null)
            {
                string[] tagData = line.Split(' ');
                object tmpPoint = new Point(Int32.Parse(tagData[1]), Int32.Parse(tagData[2]));
                ht.Add(tagData[0], tmpPoint);
            }
            tr.Close();
            RefreshTags();
        }

        private void AutoExori() // Exori enquanto estiver no cavebot
        {
            int mobs = 0;
            int playerscreen = 0;
            if (player.Mana < Int32.Parse(txtManaExori.Text)) return;

            Creature target = null;
            foreach (Creature c in new Battlelist().GetCreatures())
            {
                if (c.Z == player.Z)
                {
                    if ((Math.Abs(c.X - player.X) == 1 && c.Y == player.Y) || (Math.Abs(c.Y - player.Y) == 1 && c.X == player.X) || ((Math.Abs(c.X - player.X) == 1) && (Math.Abs(c.Y - player.Y) == 1)))
                    {
                        mobs++;
                    }
                }

            }

            foreach (Creature cp in new Battlelist().GetCreatures())
            {
                if (cp.Id != Memory.ReadInt(Addresses.Player.Id))
                {
                    if (cp.Type == (byte)Constants.Type.PLAYER)
                    {
                        Location locp = new Location(cp.X, cp.Y, cp.Z);
                        if (player.Location.DistanceTo(locp) < 4 && player.Z == locp.Z)
                        {
                            playerscreen++;
                        }
                    }
                }
            }
            if (mobs >= Int32.Parse(cmbxExoriN.Text) && playerscreen == 0) Utils.SendKeys(txtHkExori.Text);

        }



        private void AutoHeal()
        {
            if (chkHeal.Checked) // Heal
            {
                if (Memory.LicenseType == 3)
                {
                    chkHeal.Checked = false;
                    Msgbox("Sorry, you can not use this feature in Free Trial Mode.");
                    return;
                }

                int health = Int32.Parse(txtHealth.Text);
                int mana = Int32.Parse(txtMana.Text);           // Light heal
                string healhk = healHK.Text;

                int healthHeavy = Int32.Parse(txtHealthHeavy.Text);
                int manaHeavy = Int32.Parse(txtManaHeavy.Text);
                string healhkHeavy = healHKHeavy.Text;

                if (chkOnlyUH.Checked == true)
                {
                    if (player.Health <= healthHeavy || player.Health <= health)
                    {
                        Location here = new Location(player.X, player.Y, player.Z);
                        if (chkStopToUseUH.Checked)
                        {
                            player.WalkTo(here);
                            player.isWalk = 0;
                        }
                        EvaluateScript("$rclick #uh $lclick #self $wait 600");
                        Thread.Sleep(300);
                    }
                }
                else
                {


                    if (chkHeavy.Checked)
                    {

                        
                        if (player.Health <= health && player.Mana >= mana && player.Health > healthHeavy) Utils.SendKeys(healhk);

                        if (chkHeavyUH.Checked)
                        {
                            if (player.Health <= healthHeavy)
                            {
                                Location here = new Location(player.X, player.Y, player.Z);
                                if (chkStopToUseUH.Checked)
                                {
                                    player.WalkTo(here);
                                    player.isWalk = 0;
                                }
                                EvaluateScript("$rclick #uh $lclick #self $wait 600");
                                Thread.Sleep(300);
                            }
                        }
                        else
                        {

                            if (player.Health <= healthHeavy && player.Mana >= manaHeavy)
                            {
                                Utils.SendKeys(healhkHeavy);
                                Thread.Sleep(300);
                            }
                        }



                        WaitPing();
                    }
                    else
                    {
                        if (player.Health <= health && player.Mana >= mana) Utils.SendKeys(healhk);
                        Thread.Sleep(300);
                    }
                }




            }
        }


        private Point GetMapPoint(int mapX, int mapY)
        {
            Point p1 = GetPoint("#topleft");
            Point p2 = GetPoint("#bottomright");
            int tileX = (p2.X - p1.X) / 15;
            int tileY = (p2.Y - p1.Y) / 11;
            int offsetX, offsetY;
            offsetX = mapX - player.X;
            offsetY = mapY - player.Y;
            if (offsetX > 7) offsetX = 7;
            if (offsetY > 5) offsetY = 5;
            offsetX *= tileX;
            offsetY *= tileY;
            int pX = (p2.X - p1.X) / 2 + p1.X;
            int pY = (p2.Y - p1.Y) / 2 + p1.Y;
            offsetX += pX;
            offsetY += pY;

            return new Point(offsetX, offsetY);
        }


        private string GetPlayersName()
        {
            string NamesString = "";
            foreach (Creature c in new Battlelist().GetCreatures())
            {
                if (c.Id != Memory.ReadInt(Addresses.Player.Id))
                {
                    if (c.Type == (byte)Constants.Type.PLAYER)
                    {
                        if (c.Z == player.Z)
                        {
                            NamesString += c.Name + "\n";
                        }
                    }
                }
            }
            return NamesString;
        }

        private void GetBattleList()
        {
            string list = "";
            foreach (Creature c in new Battlelist().GetCreatures())
            {
                if (c.Z == player.Z && c.Name != player.Name) list += c.Name + "\n";
            }
            rtxtMainOutput.Text = list;

        }

        
        private void Cast()
        {

            while (threadCast.IsAlive)
            {


                AutoHeal();

                if (chkAutoExori.Checked)   // Auto Exori
                {
                    if (Memory.LicenseType == 3)
                    {
                        chkAutoExori.Checked = false;
                        Msgbox("Sorry, you can not use this feature in Free Trial Mode.");
                        continue;
                    }
                    AutoExori();
                    Thread.Sleep(50);
                }


                if (chkManaBurn.Checked) // Mana train
                {
                    if (Memory.LicenseType == 3)
                    {
                        chkManaBurn.Checked = false;
                        Msgbox("Sorry, you can not use this feature in Free Trial Mode.");
                        continue;
                    }
                    string burnhk = txtManaBurnHotkey.Text;
                    if (player.Mana > (player.ManaMax - 15) && player.Mana >= 25)
                    {
                        if (Memory.Otland)
                        {
                            Random rnd = new Random();
                            Thread.Sleep(rnd.Next(3000, 6000));
                        }
                        Utils.SendKeys(burnhk);
                        Thread.Sleep(1000);
                    }
                }

                if (chkHaste.Checked) // Auto Haste
                {
                    if (Memory.LicenseType == 3)
                    {
                        chkHaste.Checked = false;
                        Msgbox("Sorry, you can not use this feature in Free Trial Mode.");
                        continue;
                    }
                    string hotkey = txtHasteHK.Text;

                    bool HasteFlag = player.HasFlag(Constants.Flag.HASTE);

                    if (HasteFlag == false && player.Mana > 50)
                    {
                        Utils.SendKeys(hotkey);
                        Thread.Sleep(300);
                    }


                }


                Thread.Sleep(50);




            }


        }


        int LastTarget = 0;
        private void Main()
        {
            int Ticker = 0;
            int HousingCooldown = 0;
            int HousingInterrupts = 0;
            string LastSeeOld = "";
            while (threadMain.IsAlive)
            {
                Thread.Sleep(1);
                

                if (chkSaveLast.Checked)
                {
                    try
                    {
                        if (player.TargetId != 0) LastTarget = player.TargetId;
                    }
                    catch { }
                }




                /// Main Labels:
                /// 
                if (chkShowInfos.Checked)
                {
                    try
                    {
                        lbDirection.Text = player.Direction.ToString();
                        labelPosX.Text = player.X.ToString();
                        labelPosY.Text = player.Y.ToString();
                        labelPosZ.Text = player.Z.ToString();
                        lbHP.Text = player.Health.ToString();
                        lbMP.Text = player.Mana.ToString();
                        lbID.Text = player.Id.ToString();
                        lbFlags.Text = player.Flags.ToString();
                        lbLevel.Text = player.Level.ToString();
                        lbML.Text = player.MagicLevel.ToString();
                        lbPing.Text = Client.Ping.ToString();
                        lbAmmo.Text = player.SlotAmmo.ToString();
                        lbRing.Text = player.SlotRing.ToString();
                        lbLHand.Text = player.SlotLeftHand.ToString();
                        lbNeck.Text = player.SlotNeck.ToString();
                        lbRHand.Text = player.SlotRightHand.ToString();



                    }
                    catch (Exception e)
                    {
                    }

                    GetBattleList();

                }


                ///

                int TrayCooldown = Int32.Parse(labelTrayCooldown.Text);
                if (TrayCooldown > 0)
                {
                    TrayCooldown--;
                    labelTrayCooldown.Text = TrayCooldown.ToString();
                }


                Ticker++;
                if (HousingCooldown > 0) HousingCooldown--;
                labelTicker.Text = Ticker.ToString();
                labelHousingCooldown.Text = HousingCooldown.ToString();
                if (player != null)
                {

                    string LastSeeID = player.LastRightClickId.ToString();
                    string LastSeeCount = player.LastRightClickCount.ToString();

                    labelLastSeeID.Text = LastSeeID;
                    labelLastSeeCount.Text = LastSeeCount;

                    if (LastSeeID != LastSeeOld ) Client.StatusbarMessage = LastSeeID;

                    LastSeeOld = labelLastSeeID.Text;
                    /// Logging Part

                    /*    string StatusBarMessage = Client.StatusbarMessage.ToString();

                        if (StatusBarMessage.Contains("You")&&StatusBarMessage!=StatusBarMessageOld) Log(StatusBarMessage);

                        StatusBarMessageOld = StatusBarMessage; */



                    ///




                    if (Ticker == 20)
                    {
                        LoadPlayer();
                        RefreshTags();
                    }


                    if (chkAntiIdle.Checked & Ticker % 500 == 0) // Anti Idle
                    {
                        Utils.SendKeys("ctrl+down");
                        Utils.SendKeys("ctrl+up");
                        if (cmbxAntiIdleDirection.Text == "N") Utils.SendKeys("ctrl+up");
                        if (cmbxAntiIdleDirection.Text == "E") Utils.SendKeys("ctrl+right");
                        if (cmbxAntiIdleDirection.Text == "W") Utils.SendKeys("ctrl+left");
                        if (cmbxAntiIdleDirection.Text == "S") Utils.SendKeys("ctrl+down");


                    }



                    if (Client.Connection == 8)
                    {

                        if (PlayerOnScreen())
                        {
                            if (chkPlayerSound.Checked)
                                soundAlert.PlaySync();
                            Thread.Sleep(100);
                            if (chkPlayerFlash.Checked)
                                WinApi.FlashWindow(Memory.process.MainWindowHandle, false);
                            if (chkPlayerPause.Checked)
                                PauseAll();
                            if (chkPlayerLogout.Checked)
                                SendKeys.SendWait("^l");
                            if (chkHousingSafeStep.Checked)
                            {
                                player.WalkToInt(inHouseX, inHouseY, inHouseZ);  // Go in if player on screen
                                HousingCooldown = 300;
                                HousingInterrupts++;
                                labelInterrupts.Text = HousingInterrupts.ToString();
                            }
                            Thread.Sleep(10);
                        }
                        else
                        {

                            if (chkHousingSafeStep.Checked && HousingCooldown < 1)
                                player.WalkToInt(outHouseX, outHouseY, outHouseZ);  // Go out if no player on screen
                            Thread.Sleep(10);
                        }
                    }
                    Thread.Sleep(50);
                }
            }
        }

        private void PauseAll()
        {
            chkCavebot.Checked = false;
            chkScript.Checked = false;
        }

        // int GMTicks = 1000;


        private void NotifyPlayerScreen(string name)
        {
            int TrayCooldown = Int32.Parse(labelTrayCooldown.Text);
            if (chkTrayIcon.Checked && TrayCooldown == 0)   // TERMINAR DEPOIS
            {
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.BalloonTipTitle = "A Player appeared on screen!";
                notifyIcon.BalloonTipText = name;
                notifyIcon.ShowBalloonTip(3000);
                notifyIcon.BalloonTipClicked += new EventHandler(notifyIcon_BalloonTipClicked);
                labelTrayCooldown.Text = "150";


            }
        }

        private void NotifyLootMessage(string message)
        {
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.BalloonTipTitle = "New Loot for " + GetPlayer().Name + "!";
            notifyIcon.BalloonTipText = message;
            notifyIcon.ShowBalloonTip(3000);
        }


        private bool PlayerOnScreen()
        {
            foreach (Creature c in new Battlelist().GetCreatures())
            {
                if (c.Id != Memory.ReadInt(Addresses.Player.Id))
                {
                    if (c.Type == (byte)Constants.Type.PLAYER)
                    {
                        if (chkAnyFloor.Checked)
                        {
                            bool isSafe = false;
                            foreach (string name in lstSafe.Items)
                            {
                                if (name == c.Name)
                                {
                                    isSafe = true;
                                }

                            }

                            NotifyPlayerScreen(c.Name);

                            if (!isSafe) return true;
                        }
                        else
                        {
                            if (c.Z == player.Z)
                            {
                                bool isSafe = false;
                                foreach (string name in lstSafe.Items)
                                {
                                    if (name == c.Name)
                                    {
                                        isSafe = true;
                                    }

                                }

                                NotifyPlayerScreen(c.Name);

                                if (!isSafe) return true;
                            }
                        }
                    }
                }
            }
            return false;
        }




        private void Target(string name)
        {
            Thread.Sleep(50);

            foreach (Creature c in new Battlelist().GetCreatures())
            {
                if (c.Name == name && c.Z == player.Z)
                {
                    Point TargPos = GetMapPoint(c.X, c.Y);
                    Utils.MakeLeftClick(TargPos.X, TargPos.Y);
                }
            }
        }






        private void Script()
        {
            int ScriptTicker = 0;
            
            while (threadScript.IsAlive)
            {
                labelScriptTicker.Text = ScriptTicker.ToString();
                ScriptTicker++;
                if (chkScript.Checked && Client.Connection == 8)
                {
                    if (Memory.LicenseType == 3)
                    {
                        if (txtScript.Lines.Count() > 5)
                        {
                            Msgbox("Sorry, your scripts can not have more than 5 lines in Free Trial Mode.");
                            chkScript.Checked = false;
                            continue;
                        }
                    }                                  


                    string[] scriptLines = txtScript.Lines;
                    int NLines = scriptLines.Count();
                    if (NLines == 0) 
                    {
                        Msgbox("Write Something");
                        chkScript.Checked = false;
                        continue;
                    }

                    if (scriptIndex == NLines) { scriptIndex = 0; }
                    EvaluateScript(scriptLines[scriptIndex]);

                    if (scriptIndex<NLines) scriptIndex++;

                    Thread.Sleep(Int32.Parse(txtDelay.Text));


                }

                Thread.Sleep(50);
            }
        }

        private void ExecuteConditionalEvent(string[] cond)
        {
            // 0 = thing1
            // 1 = operator
            // 2 = thing2
            // 3 = action
            
            string str = "";
            foreach (string c in cond)
            {
                str += c;
            }
            Debug.WriteLine(str);
            if (IsConditionalEventTrue(cond[0], cond[1], cond[2]))
            {
                string cmd = "";
                for (int i = 3; i < cond.Length; i++)
                {
                    cmd += cond[i];
                    cmd += " ";
                }
                string[] command = new string[cond.Length - 3];
                for (int i = 0; i < command.Length; i++)
                {
                    command[i] = cond[i + 3];

                }
                ExecuteCommand(command);
            }
        }

        int c = 0;
        private bool IsConditionalEventTrue(string thing1, string op, string thing2)
        {
            thing1 = thing1.ToLower();
            try { 
            uint t1 = 0;
            uint t2 = UInt32.Parse(thing2);
            if (thing1.Contains('.'))
            {
                int pos = thing1.IndexOf('.');
                string part1 = thing1.Substring(0, pos);
                string part2 = thing1.Substring(pos + 1, thing1.Length - pos - 1);
                if (part1 == "$target")
                {
                    Creature creature = null;
                    foreach (Creature c in new Battlelist().GetCreatures())
                    {
                        if (c.Id == player.TargetId)
                        {
                            creature = c;
                            break;
                        }
                    }
                    if (creature == null) return false;
                    switch (part2)
                    {
                        case "hppc":
                            t1 = (uint)creature.HealthBar;
                            break;
                        case "posx":
                            t1 = (uint)creature.X;
                            break;
                        case "posy":
                            t1 = (uint)creature.Y;
                            break;
                        case "posz":
                            t1 = (uint)creature.Z;
                            break;
                        case "isWalking":
                            t1 = (uint)creature.isWalk;
                            break;
                    }
                }
            }
            else
            {
                switch (thing1)
                {
                    case "$playeronscreen":
                        if (PlayerOnScreen())
                        {
                            t1 = (uint)1;
                        }
                        else
                        {
                            t1 = (uint)0;
                        }
                        break;

                    case "$isStanding":

                        if (player.isWalk == 0)
                        {
                            c++;
                            if (c > 13) t1 = (uint)1;
                        }
                        else
                        {
                            c = 0;
                            t1 = (uint)0;
                        }

                        break;

                    case "$isTargeting":
                        if (IsTargeting())
                        {
                            t1 = (uint)1;
                        }
                        else
                        {
                            t1 = (uint)0;
                        }
                        break;

                    case "$isWalking":
                        t1 = (uint)player.isWalk;



                        break;

                    case "$hp":
                        t1 = (uint)player.Health;
                        break;
                    case "$hppc":
                        t1 = (uint)player.HealthBar;
                        break;
                    case "$mp":
                        t1 = (uint)player.Mana;
                        break;
                    case "$mppc":
                        t1 = (uint)player.ManaPercent;
                        break;
                    case "$targethp":
                        t1 = 100;
                        foreach (Creature c in new Battlelist().GetCreatures())
                        {
                            if (c.Type == (byte)Constants.Type.PLAYER)
                            {
                                if (c.Id == player.TargetId)
                                {
                                    t1 = (uint)c.HealthBar;
                                }
                            }
                        }
                        break;
                    case "$posx":
                        t1 = (uint)player.X;
                        break;
                    case "$posy":
                        t1 = (uint)player.Y;
                        break;
                    case "$posz":
                        t1 = (uint)player.Z;
                        break;
                    case "$neck":
                        t1 = player.SlotNeck;
                        break;
                    case "$rhand":
                        t1 = player.SlotRightHand;
                        break;
                    case "$lhand":
                        t1 = player.SlotLeftHand;
                        break;
                    case "$ring":
                        t1 = player.SlotRing;
                        break;
                    case "$ammo":
                        t1 = player.SlotAmmo;
                        break;
                    case "$cap":
                        t1 = (uint)player.Cap;
                        break;
                    case "$level":
                        t1 = (uint)player.Level;
                        break;
                    case "$mlevel":
                        t1 = (uint)player.MagicLevel;
                        break;
                    case "$exp":
                        t1 = (uint)player.Experience;
                        break;
                    case "$manashield":
                        t1 = Convert.ToUInt32(player.HasFlag(Constants.Flag.MANA_SHIELD));
                        break;
                    case "$haste":
                        t1 = Convert.ToUInt32(player.HasFlag(Constants.Flag.HASTE));
                        break;
                    case "$battle":
                        t1 = Convert.ToUInt32(player.HasFlag(Constants.Flag.BATTLE));
                        break;
                    case "$paralyze":
                        t1 = Convert.ToUInt32(player.HasFlag(Constants.Flag.PARALYZED));
                        break;
                    default:
                        return false;
                }
            }
            if (op == "<")
                if (t1 < t2) return true;
            if (op == ">")
                if (t1 > t2) return true;
            if (op == "<=")
                if (t1 <= t2) return true;
            if (op == ">=")
                if (t1 >= t2) return true;
            if (op == "==")
                if (t1 == t2) return true;
            if (op == "!=")
                if (t1 != t2) return true;

            return false;
        } catch
            {
            chkScript.Checked = false;
            Msgbox("There was an error on script or hotkey, please, verify if syntax is correct.");
            return false;
            }
        }

        private bool IsTargeting()
        {
            int BoxTargetID = player.TargetId;
            if (BoxTargetID == 0)
            {
                return false;
            }
            else
            {
                foreach (Creature c in new Battlelist().GetCreatures())
                {
                    if (c.Type == (byte)Constants.Type.PLAYER)
                    {
                        if (BoxTargetID == c.Id && player.Z == c.Z) return true;
                    }
                }
            }
            return false;
        }


        private Player GetPlayer()
        {
            foreach (Creature c in new Battlelist().GetCreatures())
            {
                if (c.Id == Memory.ReadInt(Addresses.Player.Id))
                {
                    return new Player(c.Address);
                }
            }
            return null;
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private bool StopAll()
        {
            chkScript.Checked = false;
            chkCavebot.Checked = false;
            //chkCond.Checked = false;
            return true;
        }

        private void Exit()
        {

            threadScript.Abort();
            threadMain.Abort();
            threadCavebot.Abort();
            threadCast.Abort();
            Application.Exit();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Exit();
        }

        private void btnRefreshTags_Click(object sender, EventArgs e)
        {
            RefreshTags();
        }

        private void lstTags_DoubleClick(object sender, EventArgs e)
        {
            if (lstTags.SelectedIndex != -1)
            {
                foreach (string k in ht.Keys)
                {
                    string theKey = k;
                    string selected = lstTags.SelectedItem.ToString();
                    int len = selected.IndexOf(' ');
                    if (selected.Substring(0, len) == theKey)
                    {
                        Point tmp;
                        tmp = (Point)ht[k.ToString()];
                        Cursor.Position = tmp;
                        return;                       
                    }
                }
            }
        }

        private void ExecuteCommand(string[] command)
        {
            int cc = 0;
            foreach (string s in command)
            {
                command[cc] = command[cc].ToLower();
                cc++;
            }

            scriptWorking = true;
            lock (this)
            {
                string sfrom, sto;
                Point pfrom, pto;
                int used = 0;

                if (command[0].Contains('_'))
                {
                    if (Memory.LicenseType == 3)
                    {
                        Client.StatusbarMessage = "You cannot use AimBot with FreeTrial License.";
                        used = 1;
                        return;
                    }
                    switch (command[0])
                    {
                        case "$target_1":
                            Target(txtT1.Text);
                            break;
                        case "$target_2":
                            Target(txtT2.Text);
                            break;
                        case "$target_3":
                            Target(txtT3.Text);
                            break;
                        case "$target_4":
                            Target(txtT4.Text);
                            break;
                        case "$target_5":
                            Target(txtT5.Text);
                            break;
                        default:
                            Client.StatusbarMessage = "Index out of Range , max is 5";
                            break;

                    }
                    used = 1;
                }
                else
                {
                    switch (command[0])
                    {
                        case "$rclick":
                            foreach (string k in ht.Keys)
                            {
                                if (command[1] == k.ToString())
                                {
                                    Point tmp;
                                    tmp = (Point)ht[k.ToString()];
                                    //Input.ClickRightMouseButton(tmp.X, tmp.Y);
                                    Utils.MakeRightClick(tmp.X, tmp.Y);
                                    used = 2;
                                    break;
                                }
                            }
                            break;
                        case "$lclick":
                            foreach (string k in ht.Keys)
                            {
                                if (command[1] == k.ToString())
                                {
                                    Thread.Sleep(50);
                                    Point tmp;
                                    tmp = (Point)ht[k.ToString()];
                                    //Input.ClickLeftMouseButton(tmp.X, tmp.Y);
                                    Utils.MakeLeftClick(tmp.X, tmp.Y);
                                    used = 2;
                                    break;
                                }
                            }
                            break;
                        case "$wait":
                            scriptWorking = false;
                            int millis = Int32.Parse(command[1]);
                            used = 2;
                            Thread.Sleep(millis);
                            break;

                        case "$key":
                            Utils.SendKeys(command[1]);
                            used = 2;
                            break;

                        case "$set":
                            string var = command[1];
                            int vaule = Int32.Parse(command[2]);

                            try
                            {
                                scriptVars.Add(var, vaule);
                            } catch(ArgumentException)
                            {
                                scriptVars[var]  = vaule;
                            }
                            used = 3;
                            break;

                        case "$add":
                            string addvar = command[1];
                            int addvaule = Int32.Parse(command[2]);


                            try
                            {
                                scriptVars[addvar]  += addvaule;
                            }
                            catch
                            {
                                Msgbox("Variable " + addvar + " is not seted yet.");
                            }
                            used = 3;
                            break;

                        case "$sub":
                            string subvar = command[1];
                            int subvaule = Int32.Parse(command[2]);


                            try
                            {
                                scriptVars[subvar] -= subvaule;
                            }
                            catch
                            {
                                Msgbox("Variable " + subvar + " is not seted yet.");
                            }
                            used = 3;
                            break;


                        case "$drag":
                            if (Memory.LicenseType == 3)
                            {
                                used = 3;
                                Client.StatusbarMessage = "You cannot use $drag with Free Trial license.";
                                break;
                            }
                            sfrom = command[1];
                            sto = command[2];
                            pfrom = GetPoint(sfrom);
                            pto = GetPoint(sto);
                            //Input.DragMouse(pfrom, pto);
                            Utils.DragMouse(pfrom, pto);
                            used = 3;
                            break;

                        case "$dragsmooth":
                            sfrom = command[1];
                            sto = command[2];
                            pfrom = GetPoint(sfrom);
                            pto = GetPoint(sto);
                            Input.DragMouseSmooth(pfrom, pto);
                            used = 3;
                            break;

                        case "$goto":
                            string scriptlabel;
                            if (command[1].Substring(command[1].Length - 1, 1) != ":")
                                scriptlabel = command[1] + ":";
                            else
                                scriptlabel = command[1];

                            int nlines = txtScript.Lines.Count();
                            try
                            {
                                for (int i = 0; i < nlines; i++)
                                {
                                    if (scriptlabel == txtScript.Lines[i].ToLower())
                                    {
                                        scriptIndex = i;
                                    }


                                }
                            }
                            catch { Msgbox("erro"); }

                                used = 2;
                                break;



                        case "$gotoline":
                            try
                            {
                                int line = Int32.Parse(command[1]);
                                lstWaypoints.SelectedIndex = line;
                                used = 2;
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                Debug.WriteLine(e.StackTrace);
                                throw;
                            }
                            break;
                        case "$gotolabel":
                            string thelabel;
                            if (command[1].Substring(command[1].Length - 1, 1) != ":")
                                thelabel = command[1] + ":";
                            else
                                thelabel = command[1];
                            try
                            {
                                for (int i = 0; i < lstWaypoints.Items.Count - 1; i++)
                                {
                                    if (thelabel == lstWaypoints.Items[i].ToString())
                                    {
                                        nextWaypoint(i);
                                        used = 2;
                                        break;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.StackTrace);
                            }
                            break;

                        case "$followtarget":
                            FollowTarget();
                            break;
                        case "$waypoint":
                            lstWaypoints.Items.Add("W, " + new Location(player.X, player.Y, player.Z));
                            used = 1;
                            break;
                        case "$node":
                            lstWaypoints.Items.Add("N, " + new Location(player.X, player.Y, player.Z));
                            used = 1;
                            break;
                        case "$ladder":
                            if (player.Direction==0) lstWaypoints.Items.Add("L, " + new Location(player.X, player.Y - 1, player.Z));
                            if (player.Direction == 1) lstWaypoints.Items.Add("L, " + new Location(player.X + 1, player.Y, player.Z));
                            if (player.Direction == 2) lstWaypoints.Items.Add("L, " + new Location(player.X, player.Y + 1, player.Z));
                            if (player.Direction == 3) lstWaypoints.Items.Add("L, " + new Location(player.X - 1, player.Y, player.Z));
                            used = 1;
                            break;

                        case "$usewpt":
                            lstWaypoints.Items.Add("U, " + new Location(player.X, player.Y, player.Z));
                            used = 1;
                            break;

                        case "$playsound":
                            soundAlert.Play();
                            used = 1;
                            break;

                        case "$takeloot":
                            TakeTheLoot();
                            used = 1;
                            break;
                        case "$stackitems":
                            autoStackItems();
                            used = 1;
                            break;

                        case "$pausescript":
                            chkScript.Checked = false;
                            used = 1;
                            break;

                        case "$pauseattacker":
                            chkAttacker.Checked = false;
                            used = 1;
                                break;
                        
                        case "$resumeattacker":
                            chkAttacker.Checked= true;
                            used=1;
                            break;

                        case "$resumescript":
                            chkScript.Checked = true;
                            used = 1;
                            break;
                        case "$pausecavebot":
                            chkCavebot.Checked = false;
                            used = 1;
                            break;
                        case "$resumecavebot":
                            chkCavebot.Checked = true;
                            used = 1;
                            break;
                        case "$target":
                            if (Memory.LicenseType == 3)
                            {
                                Client.StatusbarMessage = "You cannot use AimBot with FreeTrial License.";
                                used = 1;
                                break;
                            }
                            //get target
                            Point p1 = GetPoint("#topleft");
                            Point p2 = GetPoint("#bottomright");
                            int tileX = (p2.X - p1.X) / 14;
                            int tileY = (p2.Y - p1.Y) / 11;
                            Creature target = null;
                            foreach (Creature c in new Battlelist().GetCreatures())
                            {
                                if (c.Id == player.TargetId)
                                {
                                    target = c;
                                    break;
                                }
                            }
                            if (target == null)
                            {
                                used = 1;
                                break;
                            }
                            //get offsets
                            int offsetX, offsetY;
                            offsetX = target.X - player.X;
                            offsetY = target.Y - player.Y;
                            offsetX *= tileX;
                            offsetY *= tileY;
                            int pX = (p2.X - p1.X) / 2 + p1.X;
                            int pY = (p2.Y - p1.Y) / 2 + p1.Y;
                            offsetX += pX;
                            offsetY += pY;

                            //Input.ClickLeftMouseButton(offsetX, offsetY);
                            Utils.MakeLeftClick(offsetX, offsetY);

                            used = 1;
                            break;
                        default:
                            scriptWorking = false;
                            return;
                    }
                }
                //check remainder
                if (command.Length > used)
                {
                    string[] remainder = new string[command.Length - used];
                    for (int i = 0; i < remainder.Length; i++)
                    {
                        remainder[i] = command[i + used];
                    }
                    ExecuteCommand(remainder);
                }
            }
            scriptWorking = false;
        }

        private void FollowTarget(Creature target)
        {
            if (((Math.Abs(target.X - player.X) > 1 || Math.Abs(target.Y - player.Y) > 1)))
            {
                WriteLog("FollowTarget(Creature) Walking to: " + target.Location.ToString());
                player.WalkTo(target.Location);
            }
        }

        private void FollowTarget()
        {
            if (IsTargeting())
            {
                Creature target = null;
                foreach (Creature c in new Battlelist().GetCreatures())
                {
                    if (c.Id == player.TargetId)
                    {
                        target = c;
                        break;
                    }
                }
                if (((Math.Abs(target.X - player.X) > 1 || Math.Abs(target.Y - player.Y) > 1)))
                {
                    WriteLog("FollowTarget() Walking to: " + target.Location.ToString());
                    player.WalkTo(target.Location);
                    Thread.Sleep(100);

                }
            }
        }




        private Point GetPoint(string key)
        {
            Point p;
            p = new Point(-1, -1);
            foreach (string k in ht.Keys)
            {
                if (key.ToLower() == k.ToString().ToLower())
                {
                    try
                    {
                        p = (Point)ht[k.ToString().ToLower()];
                    }
                    catch { }
                    return p;
                }
            }
            MessageBox.Show("Could not find tag: " + key);
            return p;
        }


        private void WriteLog(string text, bool LineBreak = true)
        {
            if (consoleLogToolStripMenuItem.Checked)
            {
                if (LineBreak)
                {
                    AllocConsole();
                    Console.WriteLine(text);
                }
                else
                {
                    AllocConsole();
                    Console.Write(text);
                }
            }
            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory+ "/CavebotLogs/" + DateTime.Now.ToString("[dd-MM-yyyy]") + "cavebot.txt", "\n" + DateTime.Now.ToString(@"dd\/MM\/yyyy h\:mm tt") + " : " + text);
        }



        private uint IventoryPrint()
        {
            uint print = 0;
            foreach (Objects.Container cont in new Inventory().GetContainers())
            {
                if (cont.Number >= Int32.Parse(txtSelfContainers.Text))
                {
                    print += 10000;
                    foreach (Item i in cont.GetItems().Reverse<Item>())
                    {
                        print += i.Id;
                    }
                }
            }
            return print;
        }


        private void AttackATarget()
        {
            //foreach (Creature c in new Battlelist().GetCreatures()) // Get info of all possible targets
            //{
            //    if (c.Type == (byte)Constants.Type.CREATURE)
            //    {
            //        if (player.Location.IsAdjacentTo(c.Location,1))
            //        {
            //            Point AttackTarget = GetMapPoint(c.X, c.Y);
            //            Utils.MakeRightClick(AttackTarget.X, AttackTarget.Y);
            //            return;
            //        }
            //    }
            //}
        }

        private void CloseCorpose(Location Corpose)
        {
            WriteLog("Closing Corpose.");
            Point UseAt = GetMapPoint(Corpose.X, Corpose.Y);
            Utils.MakeRightClick(UseAt.X, UseAt.Y);
        }

        private bool OpenCorpose(Location Corpose)
        {
            int walktry = 0;
            int clientping = Client.Ping;
            if (Memory.Otland && clientping!=0) Thread.Sleep(clientping);
            else Thread.Sleep(300);

            WriteLog("Target died. Opening Corpose and starting AutoLoot");
            uint printIventory = IventoryPrint();
            WriteLog("BeforePrint = " + printIventory.ToString());
            while (true)
            {
                if (player.Location.IsAdjacentTo(Corpose, 1) || player.Z != Corpose.Z)
                {
                    if (printIventory != IventoryPrint())
                    {
                        WriteLog("Sucessfully Right Clicked at " + Corpose.ToString());
                        return true;
                    }
                    Client.StatusbarMessage = "";
                    Utils.SendKeys("ESCAPE");
                    Thread.Sleep(100);
                    WriteLog("Right clicking at corpose");               
                    Point UseAt = GetMapPoint(Corpose.X, Corpose.Y);
                    Utils.MakeRightClick(UseAt.X, UseAt.Y);
                    WaitPing(); 
                    int c = 0;
                    while (c<15)
                    {
                        if (printIventory != IventoryPrint()) break;
                        if (Client.StatusbarMessage.Contains("You cannot") || Client.StatusbarMessage.Contains("You are not"))
                        {
                            WriteLog("Cannot Open this corpose.");
                            return false;
                        }
                        Thread.Sleep(100);
                        c++;
                    }


                    if (c == 15)// && UseTries<3)//(printIventory == afterPrint) && UseTries < 3)
                    {
                        WriteLog("Right Click Failed.");//, Will try more " + (2 - UseTries).ToString() + " Times.");
                        WriteLog("Try to update your #self tag by pressing Home while tibia window is visible.");
                        WaitPing();
                        UseTries++;
                        return false;
                    }
                    else
                    {
                        WriteLog("Sucessfully Right Clicked at " + Corpose.ToString());
                        UseTries = 0;
                        return true;
                    }
                }
                else if (player.isWalk ==0)
                {
                    if (walktry > 2)
                    {
                        WriteLog("BBBBBBREAK, could not reachcorpose.");
                        return false;
                    }
                    WriteLog("Corpose is far, right clicking on it anyway [" + walktry.ToString() + "]");
                    Point UseAt = GetMapPoint(Corpose.X, Corpose.Y);
                    Utils.MakeRightClick(UseAt.X, UseAt.Y);
                    int count123 = 0;
                    while (player.isWalk==1 && count123<15)
                    { Thread.Sleep(100);
                    count123++;
                    }
                    Thread.Sleep(300);
                    walktry++;
                }
            }
        }
    


        private void autoStackItems()
        {
            WriteLog("AutoStack function has been executed.");
            WaitPing();
            if (Memory.Ver==3)
            {
                foreach (Objects.Container cont in new Inventory().GetContainers()) // Verifica se tem varios stacks do mesmo item
                {

                    if (cont.Number < Int32.Parse(txtSelfContainers.Text))
                    {
                        foreach (Item i in cont.GetItems().Reverse<Item>())
                        {
                            if (i.Location.ContainerSlot < 4 && i.Count == 100 && i.Id==3031)
                            {
                                UseItem(cont, i);
                            }
                        }
                    }
                }

                return;
            }
            foreach (Objects.Container cont in new Inventory().GetContainers()) // Verifica se tem varios stacks do mesmo item
            {

                if (cont.Number < Int32.Parse(txtSelfContainers.Text))
                {
                    foreach (Item i in cont.GetItems().Reverse<Item>())
                    {
                        if (i.Location.ContainerSlot < 4 && i.Count != 0 && i.Count != 100)
                        {
                            foreach (Item j in cont.GetItems().Reverse<Item>())
                            {
                                if (j.Location.ContainerSlot < i.Location.ContainerSlot && j.Count != 0 && j.Count != 100 && j.Id == i.Id)
                                {
                                    MoveItem(cont, i, cont, j);
                                    WaitPing();
                                    autoStackItems();
                                    return;
                                }
                            }

                        }
                    }
                }

            }
            WriteLog("AutoStack finished.");
        }

        private void MoveItem(Objects.Container contFrom, Item iFrom, Objects.Container contTo, Item iTo)
        {
            WriteLog("Moving Item :" + iFrom.Id);
            Point TakeBase = GetPoint("#FirstBpSlot");
            Point PosOffsetFrom = GetItemPosition(contFrom.Number, iFrom.Location.ContainerSlot);
            Point MoveFrom = new Point(TakeBase.X + PosOffsetFrom.X, PosOffsetFrom.Y + 370);
            Point PosOffsetTo = GetItemPosition(contTo.Number, iTo.Location.ContainerSlot);
            Point MoveTo = new Point(TakeBase.X + PosOffsetTo.X, PosOffsetTo.Y + 370);

            Utils.DragMouse(MoveFrom, MoveTo);
            Item i = iFrom;
            if (i.Count > 1)
            {
                int aj = 0;
                while (aj < 1000)
                {
                    if (aj % 10 == 0) WriteLog(aj.ToString() + " " + Client.QBoxDialog.ToString() + " " + Client.dialog.ToString());
                    if (Client.dialog == Client.QBoxDialog)
                    {
                        Thread.Sleep(50);
                        WriteLog("Sending Enter to drag item.");
                        Utils.SendKeys("Enter");
                        Thread.Sleep(50);
                        return;
                    }
                    Thread.Sleep(1);
                    aj++;
                }

            }
            if (Memory.Otland) Thread.Sleep(Client.Ping + 50);
            else Thread.Sleep(350);
            


            // LootItem
        }


        private void WaitPing()
        {
            if (Memory.Otland && Client.Ping != 0) Thread.Sleep(Client.Ping + 300);
            else Thread.Sleep(700);
        }


        private void Cavebot()
        {
            Point p1 = GetPoint("#topleft");    // new Point(324, 31);
            Point p2 = GetPoint("#bottomright"); //new Point(1416, 830);
            //Point pAttack = GetPoint("#firstbattle");
            int tileX = (p2.X - p1.X) / 14;
            int tileY = (p2.Y - p1.Y) / 11;
            int tileSize = 55;
            int CavebotTicker = 0;
            int GotoTicker = 50;
            int RetargetTicker = 300;
            int OldHP = 0;

            string OldMessage = "";

            Location Corpose = new Location();

            string CaveDebug = "";

            while (threadCavebot.IsAlive)
            {
               
                labelWaypoints.Text = "Number of lines: " + lstWaypoints.Items.Count;

                labelWpIndex.Text = lstWaypoints.SelectedIndex.ToString();
                try
                {
                    labelTargetID.Text = player.TargetId.ToString();
                }
                catch { }
                labelCavebotTicker.Text = CavebotTicker.ToString();
                CavebotTicker++;
            theLabel:
                if (scriptWorking)
                {
                    Thread.Sleep(100);
                    goto theLabel;
                }




                if (chkCavebot.Checked && Client.Connection == 8)
                {
                    if (Client.dialog == Client.QBoxDialog) 
                    {
                        Thread.Sleep(3500);
                        WriteLog("Dialog detected, pressing Enter. ########################");
                        Utils.SendKeys("ENTER");
                            WaitPing(); 
                    }

                    if (Memory.LicenseType == 3)
                    {
                        if (Memory.IsCavebotTimeOver)
                        {
                            chkCavebot.Checked = false;
                            continue;
                        }
                        else if (Memory.seconds > 180 && Memory.seconds < 180)
                        {
                            chkCavebot.Checked = false;
                            Msgbox("Cavebot is now paused due to Free Trial mode limitations.");
                            Memory.IsCavebotTimeOver = true;
                            continue;

                        }
                    }


                    CavebotDebugInfo = "";




                    if (chkAttacker.Checked)
                    {
                        /// The target will be verifyed at the beggining of the script
                        /// .
                        /// Search for possible targets in map
                        /// Attack the closest one
                        /// While Attacking Target Follow it
                        /// When Target Dies Loot or Reset Target
                        /// If no Target on Screen then Walk
                        /// 

                        // Verifica se o target ainda está vivo
                        Point nowpoint = GetPoint("#self");
                        if (nowpoint.X > 50 && nowpoint.Y > 30) savSelf = nowpoint;
                        else
                        {
                            ht.Remove("#self");
                            ht.Add("#self", savSelf);
                        }

                        bool TargetReseted = false;
                        int TargIDBox = player.TargetId;

                        if (TargIDBox != 0)
                        {
                            foreach (Creature c in new Battlelist().GetCreatures())
                            {
                                if (c.Id == TargIDBox)
                                {
                                    Corpose = c.Location;
                                    if (c.HealthBar < 1)
                                    {
                                        WriteLog("Healthbar <1  [1] ######################");
                                        if (chkTakeLoot.Checked)
                                        {
                                            if (OpenCorpose(Corpose))
                                            {
                                                Thread.Sleep(100);
                                                AttackATarget();
                                                TakeTheLoot();
                                                CloseCorpose(Corpose);
                                            }
                                            TargetReseted = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }







                        if (player.TargetId == 0 || TargetReseted) // Search and attack part  -- Essa parte roda sempre que não estiver atacando
                        {
                            CavebotDebugInfo += "Looking for Target \n";

                            int AttackRange = Int32.Parse(txtRange.Text);
                            Creature target = null;
                            //string PossibleTargetsId = "";
                            //string PossibleTargetsRange = "";


                            bool ThereIsTarget = false;
                            List<Creature> PossibleTargets = new List<Creature>();



                            foreach (Creature c in new Battlelist().GetCreatures()) // Get info of all possible targets
                            {
                                if (c.Type == (byte)Constants.Type.CREATURE)
                                {
                                    if (c.Z == player.Z && Math.Abs(c.X - player.X) <= AttackRange && Math.Abs(c.Y - player.Y) <= AttackRange)
                                    {
                                        if ((cavebotBlacklist.Contains(c.Id) == false) || player.Location.IsAdjacentTo(c.Location, 2) && TemporaryBlackList.Contains(c.Id) == false)
                                        {
                                            if (ignorelist.Contains(c.Name) == false)
                                            {
                                                PossibleTargets.Add(c);
                                                ThereIsTarget = true;
                                            }
                                        }
                                    }
                                }
                            }





                            if (ThereIsTarget) // If found nearbly target -- Se existir algum possivel target
                            {
                                WriteLog("Possible Target Found - ", false);


                                for (int i = 1; i <= AttackRange; i++)  // Esse loop serve para encontrar e selecionar o mais próximo.
                                {
                                    foreach (Creature c in PossibleTargets)
                                    {
                                        if (player.Location.IsAdjacentTo(c.Location, i) && player.Z == c.Z)//Math.Abs(c.X - player.X) <= i && Math.Abs(c.Y - player.Y) <= i)
                                        {
                                            WriteLog("Target ID: " + c.Id.ToString());
                                            target = c;
                                            break;
                                        }
                                    }
                                    if (target != null)
                                    {
                                        break;
                                    }
                                }








                                if (target != null)
                                {
                                    //&& target.isVisible == 1 && target.HealthBar > 0 &&
                                    WriteLog("Trying to click on target");
                                    Point TargetCord = GetMapPoint(target.X, target.Y);
                                    ht.Remove("#debugtarget");
                                    ht.Add("#debugtarget", TargetCord);
                                    Utils.MakeRightClick(TargetCord.X - 5, TargetCord.Y - 5);  // Right click it
                                    int cj = 0;
                                    Thread.Sleep(100);
                                    while (cj < 10)
                                    {
                                        if (player.TargetId != 0)
                                        {
                                            WriteLog("Clicked on target sucessfully \n");
                                            TemporaryBlackList.Clear();
                                            GotoTicker = 30;
                                            break;
                                        }

                                        if (Client.StatusbarMessage.Contains("First go"))
                                        {
                                            cavebotBlacklist.Add(target.Id);
                                            break;
                                        }

                                        //if (Client.StatusbarMessage.Contains("You cannot use this"))
                                        //{
                                        //    if (TryAttkTryes >= 3) TemporaryBlackList.Add(target.Id);

                                        //    if (TryAttackID == target.Id)
                                        //    {
                                        //        TryAttkTryes++;
                                        //    }
                                        //    TryAttackID = target.Id;

                                        //}
                                        Thread.Sleep(50);
                                        cj++;
                                    }
                                    if (TryAttkTryes >= 5)
                                    {
                                        TemporaryBlackList.Add(target.Id);
                                        TryAttackID = 0;
                                    }

                                    if (TryAttackID == target.Id)
                                    {
                                        WriteLog("Could not attack target, trying more: " + ((uint)5 - TryAttkTryes).ToString() + " times.");
                                        TryAttkTryes++;
                                    }
                                    TryAttackID = target.Id;

                                    Thread.Sleep(20);
                                }
                                /// Atacar com Right Click
                            }
                            else // Ou seja, se não existir nem um target possivel então andar.
                            {
                                if (chkWalker.Checked && player.isWalk == 0)
                                {
                                    CavebotDebugInfo += "Walker. \n";
                                    Walker();
                                }
                            }






                        } ///// Search and attack part End.

                        /// Enquanto estiver atacando, seguir o target e continuar atacando até ele morrer.
                        /// Para isso vamos precisar saber a posição do target e verificar se ele continua vivo.

                        else//if (player.TargetId != 0) // Essa parte só vai iniciar se o target id for diferente de 0.
                        {

                            CavebotDebugInfo += "Target is beign attacked \n";
                            bool TargetStillAlive = false;
                            Creature target = null;
                            foreach (Creature c in new Battlelist().GetCreatures())
                            {
                                if (c.Type == (byte)Constants.Type.CREATURE)
                                {
                                    if (c.Id == player.TargetId && c.Z == player.Z)
                                    {
                                        target = c;
                                        TargetStillAlive = true;
                                        break;
                                    }
                                }
                            }

                            if (TargetStillAlive && target != null) // Executar o que estiver aqui enquanto target ainda estiver vivo
                            {
                                int isstoppedc = 0;
                                do
                                {
                                    if (isstoppedc == 5)
                                    {
                                        //Thread.Sleep(500);
                                        //WriteLog("Trying to walk on mob");
                                        //player.WalkTo(target.Location);
                                        //VOLTAR
                                        Thread.Sleep(200);
                                        cavebotBlacklist.Add(target.Id);
                                        WriteLog("Cannot reach target. Target Blacklisted.");
                                        Utils.SendKeys("ESCAPE");
                                        Thread.Sleep(300);
                                        isstoppedc = 0;
                                        break;
                                    }

                                    if (player.isWalk == 0 && Client.isfollow == 1 && player.Location.IsAdjacentTo(target.Location, 1) == false)
                                    { isstoppedc++; }
                                    else { isstoppedc = 0; }
                                    //VOLTAR

                                    if (chkAttacker.Checked == false || chkCavebot.Checked == false) break;
                                    CavebotDebugInfo = "Target is still Alive \n";
                                    WaitPing();
                                    Corpose = target.Location;
                                    if (target.HealthBar < 1 || target.isVisible == 0)
                                    {
                                        WriteLog("Target Died or is Invisible [WLP] ####");
                                        if (chkTakeLoot.Checked)
                                        {
                                            if (OpenCorpose(Corpose))
                                            {
                                                Thread.Sleep(200);
                                                rtxtDebug.Text = "Taking Loot.";
                                                AttackATarget();
                                                TakeTheLoot();
                                                CloseCorpose(Corpose);
                                            }
                                            break;
                                        }
                                    }
                                    // If out of range stop attacking]
                                    if (chkStopIfOutOfRange.Checked)
                                    {
                                        int range = Int32.Parse(txtRange.Text);
                                        if (Math.Abs(target.X - player.X) > range + 1 || Math.Abs(target.Y - player.Y) > range + 1)
                                        {
                                            WriteLog("Target out of range, stopping to attack");
                                            Utils.SendKeys("ESCAPE");
                                            Thread.Sleep(200);
                                            break;
                                        }
                                    }
                                    rtxtDebug.Text = CavebotDebugInfo;

                                } while (target.HealthBar > 0 && target.Z == player.Z && player.TargetId != 0);
                            }
                            if (TargetStillAlive == false)
                            {
                                WriteLog("Reseting Target");
                                TargetReseted = true;
                                if (chkTakeLoot.Checked)
                                {
                                    WriteLog("TarvetStillAlive == false");
                                    if (OpenCorpose(Corpose))
                                    {
                                        Thread.Sleep(200);
                                        AttackATarget();
                                        TakeTheLoot();
                                        CloseCorpose(Corpose);
                                    }
                                }
                                Thread.Sleep(400);
                                player.TargetId = 0;
                            }

                            GotoTicker--;




                        }
                        //
                        //rtxtDebug.AppendText(CavebotDebugInfo);

                    }
                    else  // Se o Attacker estiver desativado então as funções de Andar e Lootear serão executadas de forma independente.
                    {
                        if (chkWalker.Checked && player.isWalk == 0)
                        {
                            Walker();
                            Thread.Sleep(1000);
                        }
                        if (chkTakeLoot.Checked)
                        {
                            Creature target = null;
                            foreach (Creature c in new Battlelist().GetCreatures())
                            {
                                if (c.Type == (byte)Constants.Type.CREATURE)
                                {
                                    if (c.Id == player.TargetId && c.Z == player.Z)
                                    {
                                        target = c;
                                        break;
                                    }
                                }
                            }
                            if (target != null)
                            {
                                do
                                {
                                    /// if player.targetid = c.id { target = c }

                                    CavebotDebugInfo = "Target is still Alive \n";
                                    WaitPing();
                                    Corpose = target.Location;
                                    if (target.HealthBar < 1 || target.isVisible == 0)
                                    {
                                        WriteLog("Target Died or is Invisible [WLP] ####");
                                        if (chkTakeLoot.Checked)
                                        {
                                            if (player.isWalk == 0)
                                            {
                                                if (OpenCorpose(Corpose))
                                                {
                                                    Thread.Sleep(200);
                                                    rtxtDebug.Text = "Taking Loot.";
                                                    AttackATarget();
                                                    TakeTheLoot();
                                                    if (player.isWalk==0) CloseCorpose(Corpose);
                                                }                                               
                                            }
                                            break;
                                        }
                                    }
                                    // If out of range stop attacking]
                                    rtxtDebug.Text = CavebotDebugInfo;
                                } while (target.HealthBar > 0 && target.Z == player.Z && player.TargetId != 0);
                            }
                        }
                        if (chkOutput.Checked)
                        {
                            rtxtDebug.Text = CavebotDebugInfo;
                        }
                    }

                }
                else if (chkSlimeTrain.Checked && Client.Connection == 8)
                {
                    while (player.TargetId == 0) // Espera o player atacar um Slime
                    {
                        Thread.Sleep(50);
                    }

                    int SlimeMotherID = player.TargetId; // Registra o ID da slimemother

                    Utils.SendKeys("ESCAPE"); // Para de atacar a slimemother
                    player.TargetId = 0;


                    while (chkSlimeTrain.Checked) // Vai continuar nesse loop até desmarcar o Slime Train
                    {
                        Thread.Sleep(100);
                        if (player.TargetId == 0) // Se ainda não estiver atacando nem um slime
                        {
                            Creature target = null;

                            foreach (Creature c in new Battlelist().GetCreatures()) // Procura slime mais próximo
                            {
                                if (c.Name == "Slime" && c.Id != SlimeMotherID && player.Location.IsAdjacentTo(c.Location, 1))
                                {
                                    target = c;  // Registra o slime
                                    break;
                                }
                            }

                            if (target != null) // Se o slime estiver registrado então ataca ele
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    Point TargetCord = GetMapPoint(target.X, target.Y);
                                    Utils.MakeRightClick(TargetCord.X, TargetCord.Y);  // Right click it
                                    Thread.Sleep(200);
                                    if (player.TargetId != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }



                }
                else if (chkMobTrain.Checked && Client.Connection == 8)
                {

                    


                }
                Thread.Sleep(50);
            }
        }

        int UseTries;
        private void Walker()
        {
            Thread.Sleep(100);

            if (lstWaypoints.Items.Count == 0)
            {
                CavebotDebugInfo += "Waypoint List is Empty. \n";
                return;
            }

            if (lstWaypoints.SelectedIndex == -1)
            {
                WriteLog("Index == -1 , Reseting Index to 0");
                lstWaypoints.SelectedIndex = 0;
            }


            string str = lstWaypoints.SelectedItem.ToString();
            if (str.Contains('$'))
            {
                WriteLog("Walker Script: " + str);

                EvaluateScript(str);
                Thread.Sleep(500);

                nextWaypoint();
                return;
            }
            else if (str.Contains(':'))
            {
                WriteLog("Walker Label: " + str);
                nextWaypoint();
            }
            else if (str.Contains(','))
            {
                string[] pos = str.Split(',');
                Location loc = new Location(Int32.Parse(pos[1]), Int32.Parse(pos[2]), Int32.Parse(pos[3]));




                if (pos[0] == "L")
                {
                    if (player.Location.IsAdjacentTo(loc) && player.Z != loc.Z)
                    {

                        WriteLog("Walker[L] Did it, Next Waypoint.");
                        Thread.Sleep(500);
                        nextWaypoint();
                    }
                    else
                    {
                        player.WalkTo(loc);
                        WriteLog("Walker[L] Going to ladder.");
                        Thread.Sleep(300);
                    }
                }
                else if ( pos[0] == "N" )
                {
                    if (player.Location.IsAdjacentTo(loc, Int32.Parse(txtRange.Text) - 1) || player.Z != loc.Z)
                    {
                        WriteLog(" Walker[N] is close to Node, next waypoint.");
                        nextWaypoint();
                        return;
                    }
                    WriteLog(" Walker[N] Walking to: " + loc.ToString());
                    player.WalkTo(loc);
                    Thread.Sleep(300);
                    //if (player.Location.IsAdjacentTo(loc, Int32.Parse(txtRange.Text)-1) || player.Z != loc.Z)
                    //{
                    //    WriteLog(" Walker[W] is close to Node, next waypoint.");
                    //    nextWaypoint();                        
                    //}
                }
                else if (pos[0] == "W")
                {
                    WriteLog(" Walker[W] Walking to: " + loc.ToString());
                    player.WalkTo(loc);
                    Thread.Sleep(300);
                    if (player.Location.IsAdjacentTo(loc, 0) || player.Z != loc.Z)
                    {
                        WriteLog(" Walker[W] Done, next waypoint.");
                        nextWaypoint();
                    }
                }
                else if (pos[0] == "R")
                {
                    WriteLog(" Walker[R] Walking to: " + loc.ToString());
                    player.WalkTo(loc);
                    Thread.Sleep(300);
                    if (player.Location.IsAdjacentTo(loc, 0) || player.Z != loc.Z)
                    {
                        if (player.Z != loc.Z)
                        {
                            nextWaypoint();
                            return;
                        }
                        uint direction = player.Direction;
                        string key;//=null;
                        switch (direction)
                        {
                            case 0:
                                key = "DOWN";
                                break;
                            case 1:
                                key = "LEFT";
                                break;
                            case 2:
                                key = "UP";
                                break;
                            case 3:
                                key = "RIGHT";
                                break;
                            default:
                                key = "UP";
                                break;
                        }
                        Utils.SendKeys(key);
                        Thread.Sleep(1000);
                        Point rope = GetPoint("#rope");
                        Utils.MakeRightClick(rope.X, rope.Y);
                        WaitPing();
                        Point coisinha = GetMapPoint(loc.X,loc.Y);
                        Utils.MakeLeftClick(coisinha.X, coisinha.Y);
                        Thread.Sleep(1000);
                        if (player.Z != loc.Z) nextWaypoint();
                    }
                }
                else if (pos[0] == "U")
                {
                    if (player.Location.IsAdjacentTo(loc, 1) || player.Z != loc.Z)
                    {
                        Client.StatusbarMessage = "";
                        Utils.SendKeys("ESCAPE");
                        Thread.Sleep(300);
                        WriteLog(" Walker[U] Right clicking at " + loc.ToString());
                        Point UseAt = GetMapPoint(loc.X, loc.Y);
                        Utils.MakeRightClick(UseAt.X, UseAt.Y);
                        if (Client.StatusbarMessage.Contains("You cannot") && UseTries < 3)
                        {
                            WriteLog("Walker[U] Right Click Fail, Will try more " + (3 - UseTries).ToString() + " Times.");
                            Thread.Sleep(333);
                            UseTries++;
                        }
                        else
                        {
                            WriteLog("Sucessfully Right Clicked at " + loc.ToString());
                            UseTries = 0;
                            nextWaypoint();
                        }
                    }
                    else
                    {
                        WriteLog(" Walker[U] Walking to: " + loc.ToString());
                        player.WalkTo(loc);
                        Thread.Sleep(300);
                    }
                }











                if (Client.StatusbarMessage == "There is no way." || Client.StatusbarMessage.Contains("First go") || Client.StatusbarMessage.Contains("Destination"))
                {
                    WriteLog(Client.StatusbarMessage + " - Selecting Next Waypoint ");
                    Client.StatusbarMessage = "";
                    nextWaypoint();
                }

            }
            else { nextWaypoint(); }

        }

        private void nextWaypoint()
        {
            WriteLog("nextWaypoint()");
            Thread.Sleep(250);
            if (lstWaypoints.Items.Count > 0)
            {
                if (lstWaypoints.SelectedIndex >= lstWaypoints.Items.Count - 1)
                    lstWaypoints.SelectedIndex = 0;
                else
                    lstWaypoints.SelectedIndex = lstWaypoints.SelectedIndex + 1;
            }
        }

        private void nextWaypoint(int num)
        {
            lstWaypoints.SelectedIndex = num;
            nextWaypoint();
        }

        private void button1_Click_5(object sender, EventArgs e)
        {
            if (cmbEmplacement.Text == "" || cmbEmplacement.Text == "C") lstWaypoints.Items.Add("W, " + new Location(player.X, player.Y, player.Z));
            if (cmbEmplacement.Text == "N") lstWaypoints.Items.Add("W, " + new Location(player.X, player.Y - 1, player.Z));
            if (cmbEmplacement.Text == "E") lstWaypoints.Items.Add("W, " + new Location(player.X + 1, player.Y, player.Z));
            if (cmbEmplacement.Text == "S") lstWaypoints.Items.Add("W, " + new Location(player.X, player.Y + 1, player.Z));
            if (cmbEmplacement.Text == "W") lstWaypoints.Items.Add("W, " + new Location(player.X - 1, player.Y, player.Z));
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            if (cmbEmplacement.Text == "" || cmbEmplacement.Text == "C") lstWaypoints.Items.Add("R, " + new Location(player.X, player.Y, player.Z));
            if (cmbEmplacement.Text == "N") lstWaypoints.Items.Add("R, " + new Location(player.X, player.Y - 1, player.Z));
            if (cmbEmplacement.Text == "E") lstWaypoints.Items.Add("R, " + new Location(player.X + 1, player.Y, player.Z));
            if (cmbEmplacement.Text == "S") lstWaypoints.Items.Add("R, " + new Location(player.X, player.Y + 1, player.Z));
            if (cmbEmplacement.Text == "W") lstWaypoints.Items.Add("R, " + new Location(player.X - 1, player.Y, player.Z));
        }


        private void btnAddWaypoint_Click(object sender, EventArgs e)
        {
            if (cmbEmplacement.Text == "" || cmbEmplacement.Text == "C") lstWaypoints.Items.Add("L, " + new Location(player.X, player.Y, player.Z));
            if (cmbEmplacement.Text == "N") lstWaypoints.Items.Add("L, " + new Location(player.X, player.Y - 1, player.Z));
            if (cmbEmplacement.Text == "E") lstWaypoints.Items.Add("L, " + new Location(player.X + 1, player.Y, player.Z));
            if (cmbEmplacement.Text == "S") lstWaypoints.Items.Add("L, " + new Location(player.X, player.Y + 1, player.Z));
            if (cmbEmplacement.Text == "W") lstWaypoints.Items.Add("L, " + new Location(player.X - 1, player.Y, player.Z));
        }

        private void btnNode_Click(object sender, EventArgs e)
        {
            if (cmbEmplacement.Text == "" || cmbEmplacement.Text == "C") lstWaypoints.Items.Add("N, " + new Location(player.X, player.Y, player.Z));
            if (cmbEmplacement.Text == "N") lstWaypoints.Items.Add("N, " + new Location(player.X, player.Y - 1, player.Z));
            if (cmbEmplacement.Text == "E") lstWaypoints.Items.Add("N, " + new Location(player.X + 1, player.Y, player.Z));
            if (cmbEmplacement.Text == "S") lstWaypoints.Items.Add("N, " + new Location(player.X, player.Y + 1, player.Z));
            if (cmbEmplacement.Text == "W") lstWaypoints.Items.Add("N, " + new Location(player.X - 1, player.Y, player.Z));
        }

        private void btnUse_Click(object sender, EventArgs e)
        {
            if (cmbEmplacement.Text == "" || cmbEmplacement.Text == "C") lstWaypoints.Items.Add("U, " + new Location(player.X, player.Y, player.Z));
            if (cmbEmplacement.Text == "N") lstWaypoints.Items.Add("U, " + new Location(player.X, player.Y - 1, player.Z));
            if (cmbEmplacement.Text == "E") lstWaypoints.Items.Add("U, " + new Location(player.X + 1, player.Y, player.Z));
            if (cmbEmplacement.Text == "S") lstWaypoints.Items.Add("U, " + new Location(player.X, player.Y + 1, player.Z));
            if (cmbEmplacement.Text == "W") lstWaypoints.Items.Add("U, " + new Location(player.X - 1, player.Y, player.Z));
        }

        private Point GetItemPosition(int cont, int slot)
        {
            int x = 0, y = 0;

            for (int i = 0; i <= cont + 1; i++)
            {
                int gui = Memory.ReadInt(Addresses.Client.GuiPointer);
                int offset = Memory.ReadInt(gui + 0x24);
                offset = Memory.ReadInt(offset + 0x24);

                for (int j = 0; j < i; j++)
                    offset = Memory.ReadInt(offset + 0x10);

                offset = Memory.ReadInt(offset + 0x44);
                y = y + Memory.ReadInt(offset + 0x20);
                y += 15;
            }
            x += 33 * slot;

            return new Point(x, y);
        }

        private void GrabItem(Objects.Container cont, Item i)
        {
            Point PutLootAt;
            if (chkDrop.Checked && (i.Id != 2148 && i.Id != 3031))
            {
                PutLootAt = GetPoint("#self");
            }
            else
            {
                PutLootAt = GetPoint("#putat");
            }            
            Point TakeBase = GetPoint("#FirstBpSlot");
            WriteLog("Item ID: " + i.Id + " -- Loot |", false);
            Point PosOffset = GetItemPosition(cont.Number, i.Location.ContainerSlot);
            Point TakeLootFrom = new Point(TakeBase.X + PosOffset.X, PosOffset.Y + 370);
            WriteLog("Dragging it from: " + TakeLootFrom.ToString() + " to: " + PutLootAt.ToString());
            Utils.DragMouse(TakeLootFrom, PutLootAt);
            if (i.Count > 1)
            {
                int aj=0;
                while(aj<1000)
                {
                if(aj%10==0) WriteLog(aj.ToString() + " " + Client.QBoxDialog.ToString()+ " " + Client.dialog.ToString());
                if(Client.dialog == Client.QBoxDialog)
                {
                    Thread.Sleep(50);
                    WriteLog("Sending Enter to grab item.");
                    Utils.SendKeys("Enter");
                    Thread.Sleep(50);
                    return;
                }
                Thread.Sleep(1);
                aj++;
                }

            }
            WaitPing();
            // LootItem
        }

        private void UseItem(Objects.Container cont, Item i)
        {
            Point PutLootAt = GetPoint("#putat");
            Point TakeBase = GetPoint("#FirstBpSlot");
            Point PosOffset = GetItemPosition(cont.Number, i.Location.ContainerSlot);
            Point TakeLootFrom = new Point(TakeBase.X + PosOffset.X, PosOffset.Y + 370);
            WriteLog("#########Using item: " + i.Id.ToString());
            Utils.MakeRightClick(TakeLootFrom.X, TakeLootFrom.Y);
            WaitPing();
            if (Client.StatusbarMessage=="You are full." && chkIncludeFoods.Checked)
            {
                if (chkDrop.Checked && (chkLootAll.Checked || lstLoot.Items.Contains(i.Id)))
                {
                    Utils.DragMouse(TakeLootFrom, GetPoint("#self"));
                    WaitPing();
                }
                else if ((chkLootAll.Checked && chkIncludeFoods.Checked ) || lstLoot.Text.Contains(i.Id.ToString()))
                {
                    GrabItem(cont, i);
                }
            }
        }

        int AntiStackoverflow = 0;
        private void TakeTheLoot()
        {

            Point PutLootAt = GetPoint("#putat");
            Point TakeBase = GetPoint("#FirstBpSlot");

            bool FoundBag = false;
            WriteLog("##########  AutoLoot Start");
        Looter:
            foreach (Objects.Container cont in new Inventory().GetContainers())// Foreach container open
            {
               // WriteLog("Container:" +cont.Number.ToString(),false);
                if (cont.Number >= Int32.Parse(txtSelfContainers.Text))  // if it is not a Self Bag
                {
                    
                    if (FoundBag == false) WriteLog("Container name:|" + cont.Name + "| -- Looting from this ");

                    foreach (Item i in cont.GetItems().Reverse<Item>()) // Foreach item in that container
                    {
                        //WriteLog("Item:" + i.Id.ToString());
                        if (foods.Contains(i.Id) && chkEatFromLoot.Checked)
                        {
                            //WriteLog("Item ID: " + i.Id + " -- Food |",false);
                            UseItem(cont, i);
                            continue;
                        }

                        if (chkLootAll.Checked)
                        {
                            if (lstLoot.Items.Contains(i.Id.ToString()) == false && lootbags.Contains(i.Id) == false)
                            {
                                if (chkIncludeFoods.Checked == false && foods.Contains(i.Id))
                                {
                                    continue;
                                }
                                GrabItem(cont, i);
                                continue;
                            }
                            // else WriteLog("Item ID: " + i.Id);
                        }
                        else
                        {
                            if (lstLoot.Items.Contains(i.Id.ToString()))
                            {
                               // WriteLog("ini Grab");
                                GrabItem(cont, i);
                               // WriteLog("FIM");
                                continue;
                            }
                            //else WriteLog("Item ID: " + i.Id);
                        }



                    }

                    //find and open bag if it exit
                    bool foundBag = false;
                    foreach (Item i in cont.GetItems().Reverse<Item>())
                    {
                        if (lootbags.Contains(i.Id))
                        {
                            WriteLog("Found a lootbag! Opening it... | ", false);
                            UseItem(cont, i);
                            foundBag = true;
                        }
                    }
                    if (foundBag && AntiStackoverflow < 3)
                    {
                        AntiStackoverflow++;
                        Thread.Sleep(500);
                        WriteLog("Lootbag open, Looting it.");
                        //TakeTheLoot();]

                        goto Looter;
                    }
                    AntiStackoverflow = 0;



                }
                //else if (FoundBag == false) WriteLog("Container name:|" + cont.Name + "|");


            }
            WriteLog("################ AutoLoot Ends");
            if (chkAutoStack.Checked) autoStackItems();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtCavebotAction.Text.Length > 0)
                lstWaypoints.Items.Add(txtCavebotAction.Text);
        }

        private void btnCavebotClear_Click(object sender, EventArgs e)
        {
            lstWaypoints.Items.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCavebotSave.Text == "") return;
            string file = txtCavebotSave.Text;
            TextWriter tw = new StreamWriter(Application.StartupPath + "/waypoints\\" + file + ".txt");
            for (int i = 0; i < lstWaypoints.Items.Count; i++)
            {
                tw.WriteLine(lstWaypoints.Items[i].ToString());
            }
            tw.Close();
            LoadCavebotScripts();
        }

        private void LootSave()
        {
            if (lstLoot.Items.Count == 0) return;
            File.WriteAllText("LootList.txt","");
            foreach (string line in lstLoot.Items)
            {
                File.AppendAllText("LootList.txt", line + "\n");
            }

        }

        private void LootLoad()
        {
            lstLoot.Items.Clear();
            foreach (string line in File.ReadAllLines("LootList.txt"))
            {
                lstLoot.Items.Add(line);
            }

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (lstCavebotScripts.SelectedIndex == -1) return;
            lstWaypoints.Items.Clear();
            string line;
            string file = lstCavebotScripts.Items[lstCavebotScripts.SelectedIndex].ToString();
            TextReader tr = new StreamReader(Application.StartupPath + "/waypoints\\" + file + ".txt");
            while ((line = tr.ReadLine()) != null)
            {
                lstWaypoints.Items.Add(line);
            }
            tr.Close();
            lstWaypoints.SelectedIndex = 0;
            txtCavebotSave.Text = file;
        }

        private void btnTagsLoad_Click(object sender, EventArgs e)
        {
            LoadTags();
        }

        private void btnTagsSave_Click(object sender, EventArgs e)
        {
            SaveTags();
        }

        private void lstWaypoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCavebotLine.Text = "Current line: " + lstWaypoints.SelectedIndex;
        }

        private void lstWaypoints_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                lstWaypoints.Items.RemoveAt(lstWaypoints.SelectedIndex);
            }
        }

        private void chkScript_CheckedChanged(object sender, EventArgs e)
        {
            if (chkScript.Checked)
            {
                txtScript.ReadOnly = true;
                scriptIndex = 0;
            }
            else
                txtScript.ReadOnly = false;
        }

        private void lstWaypoints_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lstWaypoints.SelectedIndex;
            string text = lstWaypoints.Items[index].ToString();
            if (text.Contains('$'))
            {
                if (InputBox.Show("Change action", text, ref text) == DialogResult.OK)
                {
                    lstWaypoints.Items[index] = text;
                }
            }
        }

        private void txtSafe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lstSafe.Items.Add(txtSafe.Text);
                txtSafe.Text = "";
            }
        }

        private void lstSafe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (lstSafe.SelectedIndex != -1)
                    lstSafe.Items.RemoveAt(lstSafe.SelectedIndex);
            }
        }

        private void btnLoadScript_Click(object sender, EventArgs e)
        {
            if (lstScripts.SelectedIndex == -1) return;
            string line;
            string file = lstScripts.Items[lstScripts.SelectedIndex].ToString();
            TextReader tr = new StreamReader(Application.StartupPath + "/scripts\\" + file + ".txt");
            txtScript.Text = "";
            while ((line = tr.ReadLine()) != null)
            {
                txtScript.AppendText(line + Environment.NewLine);
            }
            tr.Close();
            txtSaveScript.Text = file;
        }

        private void btnSaveScript_Click(object sender, EventArgs e)
        {
            if (txtSaveScript.Text == "") return;
            string file = txtSaveScript.Text;
            TextWriter tw = new StreamWriter(Application.StartupPath + "/scripts\\" + file + ".txt");
            foreach (string line in txtScript.Lines)
            {
                tw.WriteLine(line);
            }
            tw.Close();
            LoadScripts();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Rectangle re = Client.GameView();
            ////MessageBox.Show(Client.GameView().ToString());
            //re.Y += 28;
            //Point p = new Point(re.X, re.Y);
            //Point p2 = new Point(re.X + re.Width, re.Y + re.Height);
            //Cursor.Position = p2;
            //TakeTheLoot();
            //SendKeys.SendWait("^l");

            //string line = txtCommand.Text;
            //EvaluateScript(line);
        }


        private void EvaluateScript(string line)
        {
            if (line.Contains('&'))
            {
                string[] parts = line.Split('&');

                // se a condição presente em part[n] for true então verifica se part[n+1] é true até chegar em part[-1], então executa part[-1] 

                foreach (string parte in parts)
                {
                    if (parte == parts[parts.Length - 1])
                    {
                        EvaluateScript(parte.Trim());
                    }
                    string[] args = parte.Trim().Split(' ');
                    if (IsConditionalEventTrue(args[0], args[1], args[2]) == false)
                    {
                        return;
                    }

                }

            }
            if (line.Contains('<') || line.Contains('>') || line.Contains("<=") || line.Contains(">=") || line.Contains("==") || line.Contains("!="))
                TranslateToCE(line);
            else
                TranslateToCommand(line);
        }


        private void TranslateToCE(string ce)
        {
            string[] str = ce.Split(' ');
            ExecuteConditionalEvent(str);
        }

        private void TranslateToCommand(string command)
        {
            string[] str = command.Split(' ');
            ExecuteCommand(str);
        }


        //74 address animated text
        void anim(int x, int y, int z, int col, string msg)
        {
            //43fb10 adr
            Memory.WriteInt(0x043FB10, x);
            Memory.WriteInt(0x043FB10, y);
            Memory.WriteInt(0x043FB10, z);
            Memory.WriteInt(0x043FB10, col);
            Memory.WriteString(0x043FB10, msg);
        }

        private void fullLightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fullLightToolStripMenuItem.Checked)
            {
                Client.SetLight(true);
            }
            else
            {
                Client.SetLight(false);
            }
        }

        private void nameSpyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nameSpyToolStripMenuItem.Checked)
            {
                Client.NameSpyOn();
            }
            else
            {
                Client.NameSpyOff();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Utils.DragMouse(GetPoint("#sd"),GetPoint("#uh"));

            //Client.StatusbarMessage = player.ManaPercent.ToString();
            //TakeTheLoot();
        }

        private void onTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (onTopToolStripMenuItem.Checked)
                this.TopMost = true;
            else
                this.TopMost = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }



        private bool ConditionalEventIsValid()
        {
            if (txtThing1.Text.Length > 0 && cmbOperator.Text.Length > 0 && txtThing2.Text.Length > 0 && txtAction.Text.Length > 0)
            {
                // dataGridView1.Rows.Add(txtThing1.Text,cmbOperator.Text,txtThing2.Text,txtAction.Text);
                return true;
            }
            return false;
        }

        private void btnLabel_Click(object sender, EventArgs e)
        {
            if (txtCavebotLabel.Text.Length > 0)
                lstWaypoints.Items.Add(txtCavebotLabel.Text + ":");
        }

        private void reloadPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Memory.GetHandle();
            LoadPlayer();
            WinApi.FlashWindow(Memory.process.MainWindowHandle, false);
        }

        private void LoadPlayer()
        {
            player = GetPlayer();
            if (player != null)
            {
                this.Text = player.Name + " | Crystal Bot " + Memory.BotVersion ;
            }
            else
            {
                this.Text = "<Unknown> | Crystal Bot ";
            }
        }

        private void lstLoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
                lstLoot.Items.RemoveAt(lstLoot.SelectedIndex);
        }

        private void txtLoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                lstLoot.Items.Add(txtLoot.Text);
                txtLoot.Text = "";
            }
        }

        private void txtRange_TextChanged(object sender, EventArgs e)
        {
            int rng;

            Int32.TryParse(txtRange.Text, out rng);

            if (rng <= 0 || rng >= 5)
                txtRange.Text = "5";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkHousingSafeStep_CheckedChanged(object sender, EventArgs e)
        {
            int pX = player.X;
            int pY = player.Y;   // Get positions
            int pZ = player.Z;

            outHouseX = pX;
            outHouseY = pY;    // Set Actual Position as outHouse
            outHouseZ = pZ;

            if (cmbxHousingStepDirection.Text == "N") pY -= 1;
            if (cmbxHousingStepDirection.Text == "E") pX += 1;  // verify house direction
            if (cmbxHousingStepDirection.Text == "W") pX -= 1;
            if (cmbxHousingStepDirection.Text == "S") pY += 1;

            inHouseX = pX;
            inHouseY = pY;  // Set home position
            inHouseZ = pZ;

        }




        private void button3_Click_1(object sender, EventArgs e)
        {
            LoadHotkeys();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveHotkeys();
        }


        private void button5_Click_1(object sender, EventArgs e)
        {
            rtxtMainOutput.Text = GetPlayersName();
        }


        private void chkHaste_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            chkHeal.Checked = true;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            AutoExori();
        }


        void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            //string plname = notifyIcon.BalloonTipText;
            //if (plname != null)
            //{
            //    string[] CompositeName = plname.Split(' ');
            //    if (CompositeName[0] != plname)
            //    {
            //        plname = "";
            //        foreach (string part in CompositeName)
            //        {
            //            if (Memory.Ver == 2) plname += part + "+";
            //            else if (Memory.Ver == 3) plname += part + "%20";
            //        }
            //    }
            //    try
            //    {
            //        if (Memory.Ver == 2)
            //            System.Diagnostics.Process.Start("http://www.classicus.org/?subtopic=characters&name=" + plname);
            //        else if (Memory.Ver == 3) System.Diagnostics.Process.Start("http://eloth.net/?subtopic=characters&name=" + plname);
            //    }
            //    catch { }
            //}
        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void chkCavebot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCavebot.Checked)
            {

                if (Memory.LicenseType == 3)
                {
                    try
                    {
                    if (Memory.seconds > 360 )
                    {
                        Memory.IsCavebotTimeOver = false;
                    }

                    } catch { }

                    if (Memory.CavebotTimer > 3)
                    {
                        chkCavebot.Checked = false;
                        Msgbox("You already tried cavebot five times");
                        return;
                    }

                    if (Memory.IsCavebotTimeOver)
                    {
                        Msgbox("You can only use Cavebot for 180 seconds in Free Trial mode. \nWait more "+(360-Memory.seconds).ToString()+" seconds to test again." );
                    }
                    else
                    {
                        Msgbox("Remember that in Free Trial mode cavebot will stops working after a count of 180 seconds.");
                        Memory.TickStart = DateTime.Now.Ticks;
                        Memory.CavebotTimer++;
                    }
                }


                if (!Memory.AlreadyCavebot)
                {
                    Memory.AlreadyCavebot = true;
                    Msgbox("Remember to check Auto Follow mode on Tibia client if you want your char to follow creature");
                }
                chkSlimeTrain.Checked = false;
                chkMobTrain.Checked = false;
                RefreshTags();
                player.TargetId = 0;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {



        }

        private void btnAddCond_Click_1(object sender, EventArgs e)
        {
            if (ConditionalEventIsValid())
            {
                string[] ce = new string[4];
                ce[0] = txtThing1.Text;
                ce[1] = cmbOperator.Text;
                ce[2] = txtThing2.Text;
                ce[3] = txtAction.Text;

                string cLine = "";
                foreach (string part in ce)
                {
                    cLine += part + " ";
                }
                txtScript.AppendText(cLine + "\n");
            }
        }

        private void chkWalker_CheckedChanged(object sender, EventArgs e)
        {
            if (lstWaypoints.SelectedIndex == -1 && lstWaypoints.Items.Count != 0)
            {
                lstWaypoints.SelectedIndex = 0;
            }

        }

        private void btnWpUp_Click(object sender, EventArgs e)
        {
            MoveUp(lstWaypoints);
        }

        private void btnWpDown_Click(object sender, EventArgs e)
        {
            MoveDown(lstWaypoints);
        }

        void MoveUp(ListBox myListBox)
        {
            int selectedIndex = myListBox.SelectedIndex;
            if (selectedIndex > 0)
            {
                myListBox.Items.Insert(selectedIndex - 1, myListBox.Items[selectedIndex]);
                myListBox.Items.RemoveAt(selectedIndex + 1);
                myListBox.SelectedIndex = selectedIndex - 1;
            }
        }

        void MoveDown(ListBox myListBox)
        {
            int selectedIndex = myListBox.SelectedIndex;
            if (selectedIndex < myListBox.Items.Count - 1 & selectedIndex != -1)
            {
                myListBox.Items.Insert(selectedIndex + 2, myListBox.Items[selectedIndex]);
                myListBox.Items.RemoveAt(selectedIndex);
                myListBox.SelectedIndex = selectedIndex + 1;

            }
        }

        private void button7_Click_2(object sender, EventArgs e)
        {
            AllocConsole();
            Console.WriteLine("test");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FreeConsole();
        }

        private void consoleLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (consoleLogToolStripMenuItem.Checked)
            {
                FreeConsole();
                AllocConsole();
            }
            else
            {
                FreeConsole();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            NotifyLootMessage("teste");
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            TakeTheLoot();
        }


        private void chkTakeLoot_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTakeLoot.Checked)
            {
                Point putat = GetPoint("#putat");
                Point FirstBpSloot = GetPoint("#FirstBpSlot");

                if (putat.Y == -1 || putat.X == -1 || FirstBpSloot.X == -1 || FirstBpSloot.Y == -1)
                {
                    chkTakeLoot.Checked = false;
                    chkCavebot.Checked = false;
                    Msgbox("It appears that you did not have yet set the tags #putat and #FirstBpSlot, look at tutorial to learn how to do this.", "Oops!");

                }

                labelSelfContainers.Text = txtSelfContainers.Text;
                //if (Memory.LicenseType == 3)
                //{
                //    chkTakeLoot.Checked = false;
                //    this.Invoke(new Action(() => { MessageBox.Show(this, "You can't use AutoLoot feature with Free Trial license."); }));
                //}
            }
        }



        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {

        }





        private void txtHealth_TextChanged(object sender, EventArgs e)
        {

        }

        private void label69_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void reloadFoodstxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foods.Clear();
                using (StreamReader sr = new StreamReader("foods.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        foods.Add(uint.Parse(line));
                    }
                }
            }
            catch
            {

            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            autoStackItems();

        }

        private void button7_Click_3(object sender, EventArgs e)
        {
            SaveMain();

        }

        private void LoadMain()
        {
            try
            {
                string[] configs = File.ReadAllText("ConfigMain.txt").Split(',');

                txtHealth.Text = configs[0];
                txtHealthHeavy.Text = configs[1];
                txtMana.Text = configs[2];
                txtManaBurnHotkey.Text = configs[3];
                txtManaHeavy.Text = configs[4];
                txtHasteHK.Text = configs[5];
                healHK.Text = configs[6];
                healHKHeavy.Text = configs[7];
                txtManaExori.Text = configs[8];
                txtHkExori.Text = configs[9];
            }
            catch
            {

            }

        }

        private void SaveMain()
        {

            string content = null;

            content += txtHealth.Text;
            content += "," + txtHealthHeavy.Text;
            content += "," + txtMana.Text;
            content += "," + txtManaBurnHotkey.Text;
            content += "," + txtManaHeavy.Text;
            content += "," + txtHasteHK.Text;
            content += "," + healHK.Text;
            content += "," + healHKHeavy.Text;
            content += "," + txtManaExori.Text;
            content += "," + txtHkExori.Text;

            File.WriteAllText("ConfigMain.txt", content);

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            LoadMain();
        }




        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }



        private void Msgbox(string message, string caption = "")
        {
            this.Invoke(new Action(() => { MessageBox.Show(this, message, caption); }));
        }

        private void chkSlimeTrain_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkSlimeTrain.Checked)
            {
                Utils.SendKeys("ESCAPE");
                player.TargetId = 0;
                chkCavebot.Checked = false;
                chkMobTrain.Checked = false;
                this.Invoke(new Action(() => { MessageBox.Show(this, "First identify the Slime mother, it is the one who periodically shines. \nThen attack it. \nThe bot will stop attacking it and will only attack the other slimes.", "How to Train with Slime"); }));
            }
        }

        private void chkAutoExori_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkMobTrain_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMobTrain.Checked)
            {
                chkSlimeTrain.Checked = false;
                chkCavebot.Checked = false;
            }


        }

        private void button1_Click_3(object sender, EventArgs e)
        {
        }

        private void chkHeal_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void bUYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Forms.frmBuy();
            f.Show();
        }

        private void chkAutoExori_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void fastColoredTextBox1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_4(object sender, EventArgs e)
        {
       
        }

        private void fastColoredTextBox1_Load_1(object sender, EventArgs e)
        {

        }

        private void txtCavebotAction_TextChanged(object sender, EventArgs e)
        {

        }

        private void label73_Click(object sender, EventArgs e)
        {

        }

        private void txtCavebotAction_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtCavebotAction.Text.Length > 0)
                    lstWaypoints.Items.Add(txtCavebotAction.Text);
            }
        }

        private void txtCavebotLabel_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtCavebotLabel.Text.Length > 0)
                    lstWaypoints.Items.Add(txtCavebotLabel.Text + ":");
            }
        }

        private void txtLoot_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkAutoSelf_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoSelf.Checked)
            {
                File.WriteAllText("autoget.txt", "1");
            }
            else
            {
                Msgbox("Please, if you disable the AutoGet #self option on Main tab due to problems with the automatic option, then set these points manually as showed on the image on folder:\n #Self , #TopLeft, #BottomRight");
                File.WriteAllText("autoget.txt", "0");
            }
        }

        private void txtLoot_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void btnLootListSave_Click(object sender, EventArgs e)
        {
            LootSave();
        }

        private void btnLootListLoad_Click(object sender, EventArgs e)
        {
            LootLoad();
        }

        private void chkAntiIdle_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {

        }










    }
}
