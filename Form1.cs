using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Design;
using System.Windows.Forms.PropertyGridInternal;
using System.Windows.Forms.Design;
using Microsoft.Win32;
using Octokit;
using System.Diagnostics;
using System.Security.Principal;

namespace WeaponEd
{
    public partial class Form1 : Form
    {
		string loadedFile = "";

		public string LoadedFile
		{
			get { return loadedFile; }
			set 
			{ 
				loadedFile = value; 
				if(loadedFile != "")
				{
					this.Text = Path.GetFileName(loadedFile) + " - WeaponEd";
				}
				else
				{
					this.Text = "WeaponEd";
				}
			}
		}

        public Form1(string arg)
		{
			InitializeComponent();

			FamilyListReader.FamilyLuaPathChanged += FamilyListReader_FamilyLuaPathChanged;
			FamilyListReader.WeaponFirePathChanged += FamilyListReader_WeaponFirePathChanged;

			pnlMin.BackColor = WeaponEd.Properties.Settings.Default.Min;
			pnlMax.BackColor = WeaponEd.Properties.Settings.Default.Max;
			pnlTriggerA.BackColor = WeaponEd.Properties.Settings.Default.TriggerA;
			pnlTriggerB.BackColor = WeaponEd.Properties.Settings.Default.TriggerB;

			FamilyListReader.FamilyLuaPath = WeaponEd.Properties.Settings.Default.FamilyList;
			FamilyListReader.WeaponFire = WeaponEd.Properties.Settings.Default.WeaponFire;

			if(!IsAdministrator())
			{
				setAsDefaultwepnProgramToolStripMenuItem.Enabled = false;
				setAsDefaultwepnProgramToolStripMenuItem.Text += " (Requires Admin Rights)";
			}

			if(File.Exists(arg))
			{
				OpenFile(arg);
			}
        }

		void FamilyListReader_WeaponFirePathChanged(object sender, EventArgs e)
		{
			lblWeaponFire.Text = FamilyListReader.WeaponFire;
		}

		void FamilyListReader_FamilyLuaPathChanged(object sender, EventArgs e)
		{
			lblFamilyLua.Text = FamilyListReader.FamilyLuaPath;
		}

		private void SetAsDefaultProgram()
		{
			String myExecutable = System.Reflection.Assembly.GetEntryAssembly().Location;
			String command = "\"" + myExecutable + "\"" + " \"%L\"";

			string appPath = @"Applications\WeaponEd.exe";			
			string openPath = @"\shell\open\command";			
			string editPath = @"\shell\edit\command";

			string autoPath = @"wepn_auto_file";

			using (var key = Registry.ClassesRoot.CreateSubKey(".wepn"))
			{
				key.SetValue("", "wepn_auto_file");
			}

			using (var key = Registry.ClassesRoot.CreateSubKey(appPath + openPath))
			{
				key.SetValue("", command);
			}

			using (var key = Registry.ClassesRoot.CreateSubKey(appPath + editPath))
			{
				key.SetValue("", command);
			}

			using (var key = Registry.ClassesRoot.CreateSubKey(autoPath + openPath))
			{
				key.SetValue("", command);
			}

			using (var key = Registry.ClassesRoot.CreateSubKey(autoPath + editPath))
			{
				key.SetValue("", command);
			}

			//=============================================================================================

			using (var key = Registry.ClassesRoot.CreateSubKey(".wepn" + openPath))
			{
				key.SetValue("", command);
			}

			string softwarePath = @"Software\Classes\";
			using (var key = Registry.CurrentUser.CreateSubKey(softwarePath + ".wepn"))
			{
				key.SetValue("", "wepn_auto_file");
			}

			using (var key = Registry.ClassesRoot.CreateSubKey(softwarePath + "wepn_auto_file" + openPath))
			{
				key.SetValue("", command);
			}

			using (var key = Registry.ClassesRoot.CreateSubKey(softwarePath + "wepn_auto_file" + editPath))
			{
				key.SetValue("", command);
			}

			string filePath = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.wepn\";
			using (var key = Registry.CurrentUser.CreateSubKey(filePath + "OpenWithList"))
			{
				var list = key.GetValue("MRUList");
				if (list != null)
				{
					string data = list.ToString();
					bool foundWeponEd = false;
					char index = ' ';
					foreach (char item in data)
					{
						if ((key.GetValue(item.ToString()).ToString() == "WeaponEd.exe"))
						{
							foundWeponEd = true;
							index = item;
						}
					}

					if (foundWeponEd)
					{
						//Remove it from the existing position in the list and put it at the top
						data = data.Remove(data.IndexOf(index), 1);
						data = index + data;
					}
					else
					{
						//It didn't already exist so let's add it using the next letter in the alphabet and then insert it at the top
						char lastChar = data[data.Length - 1];
						lastChar++;
						key.SetValue(lastChar.ToString(), "WeaponEd.exe");
						data = lastChar + data;
					}
					key.SetValue("MRUList", data);
				}
				else
				{
					key.SetValue("a", "WeaponEd.exe");
					key.SetValue("MRUList", "a");
				}
			}

			using (var key = Registry.CurrentUser.CreateSubKey(filePath + "OpenWithProgids"))
			{
				key.SetValue("wepn_auto_file", new byte[] { }, RegistryValueKind.None);
			}
		}

		private void CheckVersion()
		{
			var client = new Octokit.GitHubClient(new ProductHeaderValue("WeaponEd"));
			var releases = client.Release.GetAll("xercodo", "WeaponEd");
			var latest = releases.Result[0];

			Version ver = Assembly.GetExecutingAssembly().GetName().Version;
			string currentVersion = ver.Major + "." + ver.Minor;
			if (currentVersion != latest.TagName)
			{
				DialogResult result =  MessageBox.Show(this, "There's a newer version of WeaponEd!\r\n\r\n" + 
					"Current: " + currentVersion + "\r\n" + 
					"Latest Version: " + latest.TagName + "\r\n\r\n" + 
					"Do you want to visit the release page now?", 
					"Newer Version", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
				if(result == System.Windows.Forms.DialogResult.Yes)
				{
					Process proc = new Process();
					proc.StartInfo = new ProcessStartInfo(latest.HtmlUrl);
					proc.Start();
					this.Close();
				}
			}
		}

		private void OpenFile(string file)
		{
			weaponDefine = new WeaponDefinition();
			weaponDefine.AngleUpdate += weaponDefine_AngleUpdate;
			propertyGrid1.SelectedObject = weaponDefine;
			SetLabelColumnWidth(propertyGrid1, 200);

			LoadedFile = file;

			StreamReader reader = new StreamReader(file);
			bool functionStarted = false;
			string functionName = "";
			string functionLine = "";

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();

				if(line.Contains("--"))
				{
					line = line.Substring(0, line.IndexOf("--"));
				}

				if(line.Contains("("))
				{
					functionStarted = true;
					functionName = line.Substring(0, line.LastIndexOf('('));
					functionLine = "";
				}

				if (line.Contains(");") && functionStarted)
				{
					functionLine += line;

					switch (functionName)
					{
						case "StartWeaponConfig":
							weaponDefine.ParseAndLoadConfig(functionLine);
							break;
						case "setAngles":
							weaponDefine.ParseAndLoadAngles(functionLine);
							break;
						case "AddWeaponResult":
							weaponDefine.ParseAndLoadResult(functionLine);
							break;
						case "setPenetration":
							weaponDefine.ParseAndLoadPenetration(functionLine);
							break;
						case "setAccuracy":
							weaponDefine.ParseAndLoadAccuracy(functionLine);
							break;
						case "setMiscValues":
							weaponDefine.ParseAndLoadMisc(functionLine);
							break;
						case "addAnimTurretSound":
							weaponDefine.ParseAndLoadSound(functionLine);
							break;
						default:
							break;
					}
					functionStarted = false;
				}
				else if(functionStarted)
				{
					functionLine += line;
				}
			}

			reader.Close();

			pnlHorizPreview.Refresh();
			pnlVertPreview.Refresh();
		}

		void weaponDefine_AngleUpdate(object sender, EventArgs e)
		{
			pnlHorizPreview.Refresh();
			pnlVertPreview.Refresh();
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{
			DrawArcHoriz(e.Graphics);
		}

		private void panel2_Paint(object sender, PaintEventArgs e)
		{
			DrawArcVert(e.Graphics);
		}

		private void DrawArcHoriz(Graphics g)
		{
			if (pnlMin.Tag == null || pnlMax.Tag == null || pnlTriggerA.Tag == null || pnlTriggerB.Tag == null)
				return;

			float startAngle = -90;

			// Create rectangle to bound ellipse.
			Rectangle rect = new Rectangle(0, 20, 200, 200);

			float angleA = (weaponDefine.Maxazimuth);
			float angleB = (weaponDefine.Minazimuth);

			// Draw arc to screen.
			g.FillPie(pnlMax.Tag as Brush, rect, startAngle, angleA);
			g.FillPie(pnlMin.Tag as Brush, rect, startAngle, angleB);

			if (chkTrigger.Checked)
			{
				g.FillPie(pnlTriggerA.Tag as Brush, rect, startAngle, weaponDefine.Triggerhappy);
				g.FillPie(pnlTriggerA.Tag as Brush, rect, startAngle, -weaponDefine.Triggerhappy);
				if (angleA != 0)
				{
					float temp = startAngle + angleA;
					g.FillPie(pnlTriggerB.Tag as Brush, rect, temp, weaponDefine.Triggerhappy);
				}

				if (angleB != 0)
				{
					float temp = startAngle + angleB;
					g.FillPie(pnlTriggerB.Tag as Brush, rect, temp, -weaponDefine.Triggerhappy);
				}
			}
		}

		private void DrawArcVert(Graphics g)
		{
			if (pnlMin.Tag == null || pnlMax.Tag == null || pnlTriggerA.Tag == null || pnlTriggerB.Tag == null)
				return;

			float startAngle = 0;

			// Create rectangle to bound ellipse.
			Rectangle rect = new Rectangle(-5, -5, 134, 134);

			float angleA = -(weaponDefine.Mindeclination);
			float angleB = -(weaponDefine.Maxdeclination);

			// Draw arc to screen.
			g.FillPie(pnlMin.Tag as Brush, rect, startAngle, angleA);
			g.FillPie(pnlMax.Tag as Brush, rect, startAngle, angleB);

			if (chkTrigger.Checked)
			{
				g.FillPie(pnlTriggerA.Tag as Brush, rect, startAngle, weaponDefine.Triggerhappy);
				g.FillPie(pnlTriggerA.Tag as Brush, rect, startAngle, -weaponDefine.Triggerhappy);
				if (angleA != 0)
				{
					float temp = startAngle + angleA;
					g.FillPie(pnlTriggerB.Tag as Brush, rect, temp, weaponDefine.Triggerhappy);
				}

				if (angleB != 0)
				{
					float temp = startAngle + angleB;
					g.FillPie(pnlTriggerB.Tag as Brush, rect, temp, -weaponDefine.Triggerhappy);
				}
			}
		}

		private void panel3_Click(object sender, EventArgs e)
		{
			Panel source = (Panel)sender;
			colorDialog1.Color = source.BackColor;
			DialogResult result = colorDialog1.ShowDialog();
			if(result == System.Windows.Forms.DialogResult.OK)
			{
				source.BackColor = colorDialog1.Color;

				WeaponEd.Properties.Settings.Default.Min = pnlMin.BackColor;
				WeaponEd.Properties.Settings.Default.Max = pnlMax.BackColor;
				WeaponEd.Properties.Settings.Default.TriggerA = pnlTriggerA.BackColor;
				WeaponEd.Properties.Settings.Default.TriggerB = pnlTriggerB.BackColor;
				WeaponEd.Properties.Settings.Default.Save();

				pnlHorizPreview.Refresh();
				pnlVertPreview.Refresh();
			}
		}

		private void pnl_BackColorChanged(object sender, EventArgs e)
		{
			Panel source = (Panel)sender;
			source.Tag = new SolidBrush(Color.FromArgb(128, source.BackColor));
		}

		public static void SetLabelColumnWidth(PropertyGrid grid, int width)
		{
			if (grid == null)
				return;

			FieldInfo fi = grid.GetType().GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);
			if (fi == null)
				return;

			Control view = fi.GetValue(grid) as Control;
			if (view == null)
				return;

			MethodInfo mi = view.GetType().GetMethod("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic);
			if (mi == null)
				return;
			mi.Invoke(view, new object[] { width });
		}

		private void chkTrigger_CheckedChanged(object sender, EventArgs e)
		{
			pnlHorizPreview.Refresh();
			pnlVertPreview.Refresh();
		}

		private void selectFamilyListluaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = openFileDialog2.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				WeaponEd.Properties.Settings.Default.FamilyList = openFileDialog2.FileName;
				WeaponEd.Properties.Settings.Default.Save();
				FamilyListReader.FamilyLuaPath = WeaponEd.Properties.Settings.Default.FamilyList;
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = saveFileDialog1.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				LoadedFile = saveFileDialog1.FileName;
				StreamWriter writer = new StreamWriter(LoadedFile);
				writer.Write(weaponDefine.ToString());
				writer.Close();
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			SetLabelColumnWidth(propertyGrid1, 200);

			CheckVersion();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBox1 aboutBox = new AboutBox1();
			aboutBox.ShowDialog();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StreamWriter writer = new StreamWriter(LoadedFile);
			writer.Write(weaponDefine.ToString());
			writer.Close();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = openFileDialog1.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				OpenFile(openFileDialog1.FileName);
			}
		}

		private void setAsDefaultwepnProgramToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetAsDefaultProgram();
			MessageBox.Show("Done!");
		}

		public static bool IsAdministrator()
		{
			var identity = WindowsIdentity.GetCurrent();
			var principal = new WindowsPrincipal(identity);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		private void checkVersionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckVersion();
		}

		private void selectWeaponFireFolderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = folderBrowserDialog1.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				WeaponEd.Properties.Settings.Default.WeaponFire = folderBrowserDialog1.SelectedPath;
				WeaponEd.Properties.Settings.Default.Save();
				FamilyListReader.WeaponFire = WeaponEd.Properties.Settings.Default.WeaponFire;
				FamilyListReader.GetWeaponFireEffects();
			}
		}
    }

	public static class AttributeUtils
	{
		public static T Assign<T>(string value)
		{

			DataTable table = new DataTable();

			if (typeof(T) == typeof(string))
			{
				return (T)Convert.ChangeType(value, typeof(T));
			}
			else if (typeof(T) == typeof(float))
			{
				return (T)Convert.ChangeType(Convert.ToDecimal(table.Compute(value, "")), typeof(T));
			}
			else if (typeof(T) == typeof(int))
			{
				return (T)Convert.ChangeType(Convert.ToInt32(table.Compute(value, "")), typeof(T));
			}
			else if (typeof(T) == typeof(bool))
			{
				return (T)Convert.ChangeType(Convert.ToBoolean(table.Compute(value, "")), typeof(T));
			}
			else
			{
				return (T)Convert.ChangeType(null, typeof(T));
			}
		}

		public static string Check<T>(Expression<Func<T>> expr)
		{
			var body = ((MemberExpression)expr.Body);
			string returnstring = "";
			returnstring += body.Member.Name.PadRight(43, ' ');
			returnstring += " : " + ((FieldInfo)body.Member)
		   .GetValue(((ConstantExpression)body.Expression).Value).ToString().PadLeft(15);

			return returnstring + "\r\n";
		}
	}

	public class WeaponResult
	{
		#region Properties

		private string weapon = "";
		[CategoryAttribute("Types"),
		Browsable(false),
		DisplayName("Weapon Name")]
		public string Weapon
		{
			get { return weapon; }
			set { weapon = value; }
		}

		private string condition = "";
		[CategoryAttribute("Types"),
		TypeConverter(typeof(WeaponTypeConverter)),
		DisplayName("Condition")]
		public string Condition
		{
			get { return condition; }
			set { condition = value; }
		}

		private string effect = "";
		[CategoryAttribute("Types"),
		TypeConverter(typeof(WeaponTypeConverter)),
		DisplayName("Effect")]
		public string Effect
		{
			get { return effect; }
			set { effect = value; }
		}

		private string target = "";
		[CategoryAttribute("Types"),
		TypeConverter(typeof(WeaponTypeConverter)),
		DisplayName("Target")]
		public string Target
		{
			get { return target; }
			set { target = value; }
		}

		private float minimumeffect = 0;
		[CategoryAttribute("Values"),
		DisplayName("Min Effect")]
		public float Minimumeffect
		{
			get { return minimumeffect; }
			set { minimumeffect = value; }
		}

		private float maximumeffect = 0;
		[CategoryAttribute("Values"),
		DisplayName("Max Effect")]
		public float Maximumeffect
		{
			get { return maximumeffect; }
			set { maximumeffect = value; }
		}

		private string spawnedweaponeffect = "";
		[CategoryAttribute("Types"),
		DisplayName("Spawned Effect")]
		public string Spawnedweaponeffect
		{
			get { return spawnedweaponeffect; }
			set { spawnedweaponeffect = value; }
		}

		#endregion

		private string[] ParseSetup(string source)
		{
			int first = source.IndexOf('(');
			int last = source.IndexOf(')');
			string data = source.Substring(first + 1, last - first - 1);

			string[] commaData = data.Split(new char[] { ',' }, StringSplitOptions.None);

			//Lazy removal of quotes
			for (int i = 0; i < commaData.Length; i++)
			{
				string entry = commaData[i];
				if (entry.Contains("\""))
				{
					entry = entry.Substring(1, entry.Length - 2);
				}
				commaData[i] = entry;
			}

			return commaData;
		}

		public void ParseAndLoad(string source)
		{
			string[] data = ParseSetup(source);

			int counter = 0;

			weapon = AttributeUtils.Assign<string>(data[counter++]);
			condition = AttributeUtils.Assign<string>(data[counter++]);
			effect = AttributeUtils.Assign<string>(data[counter++]);
			target = AttributeUtils.Assign<string>(data[counter++]);
			minimumeffect = AttributeUtils.Assign<float>(data[counter++]);
			maximumeffect = AttributeUtils.Assign<float>(data[counter++]);
			spawnedweaponeffect = AttributeUtils.Assign<string>(data[counter++]);
		}

		public override string ToString()
		{
			string returnString = "";
			returnString += Condition + ":" + Effect + " on " + Target;
			return returnString;
		}
	}

	public class AccuracyProfile
	{
		#region Properties

		private string familyname = "";
		[CategoryAttribute("Accuracy"),
		TypeConverter(typeof(WeaponTypeConverter)),
		DisplayName("Family Name")]
		public string Familyname
		{
		  get { return familyname; }
		  set { familyname = value; }
		}

		private float value = 0;
		[CategoryAttribute("Accuracy"),
		DisplayName("Accuracy Value")]
		public float Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		private bool alwaysdamage = false;
		[CategoryAttribute("Accuracy"),
		DisplayName("Always Does Damage")]
		public bool Alwaysdamage
		{
			get { return alwaysdamage; }
			set { alwaysdamage = value; }
		}

		#endregion

		public override string ToString()
		{
			return Familyname + " : " + value + " : " + alwaysdamage;
		}
	}

	public class PenetrationProfile
	{
		#region Properties

		private string armorfamilyname = "";
		[CategoryAttribute("Penetration"),
		TypeConverter(typeof(WeaponTypeConverter)),
		DisplayName("Family Name")]
		public string Armorfamilyname
		{
			get { return armorfamilyname; }
			set { armorfamilyname = value; }
		}

		private float value = 0;
		[CategoryAttribute("Penetration"),
		DisplayName("Penetration Value")]
		public float Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		#endregion

		public override string ToString()
		{
			return armorfamilyname + " : " + value;
		}
	}

	public class WeaponDefinition : Component
	{
		#region Properties

		#region Main Properties

		private string weaponname = "";
		[CategoryAttribute(" Types"),
		Browsable(false),
		DisplayName("Weapon Name"),
		Description("Reference to the weapon. Generally, this will be NewWeaponType.")]
		public string Weaponname
		{
			get { return weaponname; }
			set { weaponname = value; }
		}

		private string weapontype = "";
		[CategoryAttribute(" Types"),
		TypeConverter(typeof(WeaponTypeConverter)),
		DisplayName("Weapon Type"),
		Description("How the weapon tracks targets. Possible values are \"Gimble\", " + 
			"\"AnimatedTurret\", or \"Fixed\". \"Gimble\" indicates the weapon is not " + 
			"in a turret but can aim at targets within a certain cone. \"AnimatedTurret\" " + 
			"indicates that the weapon is mounted in a visible, moving turret. \"Fixed\" " + 
			"indicates the weapon can only fire in a fixed (unchanging) direction, such " +
			 "as missile and mine launchers.")]
		public string Weapontype
		{
			get { return weapontype; }
			set { weapontype = value; }
		}

		private string weaponfiretype = "";
		[CategoryAttribute(" Types"),
		TypeConverter(typeof(WeaponTypeConverter)),
		DisplayName("Fire Type"),
		Description(" Type of projectile, possible values are \"InstantHit\", \"Bullet\"," + 
			" \"Mine\", \"Missile\", or \"SphereBurst\". \"InstantHit\" is generally used" + 
			" with beam weapons, and causes the weapon's damage or other effects to take " + 
			"place immediately upon firing. \"Bullet\" indicates an unguided projectile. " +
			 "\"Mine\" indicates the projectile is dropped where the ship is located, and " + 
			 "waits to detonate when an enemy ship moves nearby. \"Missile\" indicates a " + 
			 "guided projectile. \"SphereBurst\" indicates an area of effect weapon. " + 
			 "\"SphereBurst\" weapons require two \".wepn\" files to be defined. The first " + 
			 "\".wepn\" file defines how the weapon is fired, while the second \".wepn\" file " + 
			 "defines the effects of a hit or miss. See \"hgn_smallemp.wepn\" and " + 
			 "\"hgn_smallempburst.wepn\" for an example.")]
		public string Weaponfiretype
		{
			get { return weaponfiretype; }
			set { weaponfiretype = value; }
		}

		private string weaponfirename = "";
		[CategoryAttribute(" Types"), 
		TypeConverter(typeof(WeaponTypeConverter)),
		DisplayName("Fire Name"),
		Description("This is the description for SpaceKey")]
		public string Weaponfirename
		{
			get { return weaponfirename; }
			set { weaponfirename = value; }
		}
		
		private string activation = "";
		[CategoryAttribute(" Types"),
		TypeConverter(typeof(WeaponTypeConverter)),
		DisplayName("Activation"),
		Description("This is the description for SpaceKey")]
		public string Activation
		{
			get { return activation; }
			set { activation = value; }
		}

		private float weaponfirespeed = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Fire Speed"),
		Description("This is the description for SpaceKey")]
		public float Weaponfirespeed
		{
			get { return weaponfirespeed; }
			set { weaponfirespeed = value; }
		}

		private float weaponfirerange = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Fire Range"),
		Description("This is the description for SpaceKey")]
		public float Weaponfirerange
		{
			get { return weaponfirerange; }
			set { weaponfirerange = value; }
		}

		private float weaponfireradius = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Fire Radius"),
		Description("This is the description for SpaceKey")]
		public float Weaponfireradius
		{
			get { return weaponfireradius; }
			set { weaponfireradius = value; }
		}

		private float weaponfirelifetime = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Beam Lifetime"),
		Description("This is the description for SpaceKey")]
		public float Weaponfirelifetime
		{
			get { return weaponfirelifetime; }
			set { weaponfirelifetime = value; }
		}

		private float weaponfiremisc1 = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Beam Spool Up"),
		Description("This is the description for SpaceKey")]
		public float Weaponfiremisc1
		{
			get { return weaponfiremisc1; }
			set { weaponfiremisc1 = value; }
		}

		private int weaponfireaxis = 0;
		[CategoryAttribute("Int Values"),
		DisplayName("Fire Axis (Missile Only)"),
		Description("This is the description for SpaceKey")]
		public int Weaponfireaxis
		{
			get { return weaponfireaxis; }
			set { weaponfireaxis = value; }
		}

		private int maxeffectsspawned = 0;
		[CategoryAttribute("Int Values"),
		DisplayName("Effects Spawned"),
		Description("This is the description for SpaceKey")]
		public int Maxeffectsspawned
		{
			get { return maxeffectsspawned; }
			set { maxeffectsspawned = value; }
		}

		private bool usevelocitypred = false;
		[CategoryAttribute("Flags"),
		Editor(typeof(CheckBoxUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Use Velocity Prediction"),
		Description("This is the description for SpaceKey")]
		public bool Usevelocitypred
		{
			get { return usevelocitypred; }
			set { usevelocitypred = value; }
		}

		private bool checklineoffire = false;
		[CategoryAttribute("Flags"),
		Editor(typeof(CheckBoxUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Check Line of Fire"),
		Description("This is the description for SpaceKey")]
		public bool Checklineoffire
		{
			get { return checklineoffire; }
			set { checklineoffire = value; }
		}

		private float firetime = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Fire Time"),
		Description("This is the description for SpaceKey")]
		public float Firetime
		{
			get { return firetime; }
			set { firetime = value; }
		}

		private float burstfiretime = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Burst Fire Time"),
		Description("This is the description for SpaceKey")]
		public float Burstfiretime
		{
			get { return burstfiretime; }
			set { burstfiretime = value; }
		}

		private float burstwaittime = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Burst Wait Time"),
		Description("This is the description for SpaceKey")]
		public float Burstwaittime
		{
			get { return burstwaittime; }
			set { burstwaittime = value; }
		}

		private bool shootatsecondaries = false;
		[CategoryAttribute("Flags"),
		Editor(typeof(CheckBoxUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Shoot Secondaries"),
		Description("This is the description for SpaceKey")]
		public bool Shootatsecondaries
		{
			get { return shootatsecondaries; }
			set { shootatsecondaries = value; }
		}

		private bool shootatsurroundings = false;
		[CategoryAttribute("Flags"),
		Editor(typeof(CheckBoxUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Shoot Surroundings"),
		Description("This is the description for SpaceKey")]
		public bool Shootatsurroundings
		{
			get { return shootatsurroundings; }
			set { shootatsurroundings = value; }
		}

		private float maxazimuthspeed = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Max Rotation Speed"),
		Description("This is the description for SpaceKey")]
		public float Maxazimuthspeed
		{
			get { return maxazimuthspeed; }
			set { maxazimuthspeed = value; }
		}

		private float maxdeclinationspeed = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Max Pitch Speed"),
		Description("This is the description for SpaceKey")]
		public float Maxdeclinationspeed
		{
			get { return maxdeclinationspeed; }
			set { maxdeclinationspeed = value; }
		}

		private float speedmultiplierwhenpointingattarget = 0;
		[CategoryAttribute("Float Values"),
		DisplayName("Targeted Tracking Speed Mulitplier"),
		Description("This is the description for SpaceKey")]
		public float Speedmultiplierwhenpointingattarget
		{
			get { return speedmultiplierwhenpointingattarget; }
			set { speedmultiplierwhenpointingattarget = value; }
		}

		private string weaponshieldpenetration = "";
		[CategoryAttribute(" Types"),
		TypeConverter(typeof(WeaponTypeConverter)),
		DisplayName("Shield Penetration"),
		Description("This is the description for SpaceKey")]
		public string Weaponshieldpenetration
		{
			get { return weaponshieldpenetration; }
			set { weaponshieldpenetration = value; }
		}

		private bool tracktargetsoutsiderange = false;
		[CategoryAttribute("Flags"),
		Editor(typeof(CheckBoxUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Track Targets out of Range"),
		Description("This is the description for SpaceKey")]
		public bool Tracktargetsoutsiderange
		{
			get { return tracktargetsoutsiderange; }
			set { tracktargetsoutsiderange = value; }
		}

		private bool waituntillcoderedstate = false;
		[CategoryAttribute("Flags"),
		Editor(typeof(CheckBoxUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Wait for Code Red"),
		Description("This is the description for SpaceKey")]
		public bool Waituntillcoderedstate
		{
			get { return waituntillcoderedstate; }
			set { waituntillcoderedstate = value; }
		}

		private int instanthitthreshold = 0;
		[CategoryAttribute("Int Values"),
		DisplayName("Punch-through Threshold"),
		Description("This is the description for SpaceKey")]
		public int Instanthitthreshold
		{
			get { return instanthitthreshold; }
			set { instanthitthreshold = value; }
		}

		#endregion

		#region Angles
		//==================================================================== Angles

		private string objecttype;
		[CategoryAttribute("Angles"),
		Browsable(false),
		DisplayName("Name")]
		public string Objecttype
		{
			get { return objecttype; }
			set { objecttype = value; }
		}

		private float triggerhappy = 0;
		[CategoryAttribute("Angles"),
		Editor(typeof(NumericUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Trigger Happy")]
		public float Triggerhappy
		{
			get { return triggerhappy; }
			set 
			{ 
				triggerhappy = value; 
				if(AngleUpdate != null)
				{
					AngleUpdate(this, EventArgs.Empty);
				}
			}
		}

		private float minazimuth = 0;
		[CategoryAttribute("Angles"),
		Editor(typeof(NumericUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Min Rotation")]
		public float Minazimuth
		{
			get { return minazimuth; }
			set 
			{
				minazimuth = value; 
				if (AngleUpdate != null)
				{
					AngleUpdate(this, EventArgs.Empty);
				}
			}
		}

		private float maxazimuth = 0;
		[CategoryAttribute("Angles"),
		Editor(typeof(NumericUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Max Rotation")]
		public float Maxazimuth
		{
			get { return maxazimuth; }
			set 
			{
				maxazimuth = value;
				if (AngleUpdate != null)
				{
					AngleUpdate(this, EventArgs.Empty);
				}
			}
		}

		private float mindeclination = 0;
		[CategoryAttribute("Angles"),
		Editor(typeof(NumericUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Min Pitch")]
		public float Mindeclination
		{
			get { return mindeclination; }
			set 
			{ 
				mindeclination = value;
				if (AngleUpdate != null)
				{
					AngleUpdate(this, EventArgs.Empty);
				}
			}
		}

		private float maxdeclination = 0;
		[CategoryAttribute("Angles"),
		Editor(typeof(NumericUIProp),
			typeof(System.Drawing.Design.UITypeEditor)),
		DisplayName("Max Pitch")]
		public float Maxdeclination
		{
			get { return maxdeclination; }
			set 
			{
				maxdeclination = value;
				if (AngleUpdate != null)
				{
					AngleUpdate(this, EventArgs.Empty);
				}
			}
		}

		#endregion

		#region Weapon Results, Penetration, and Accuracy
		//==================================================================== WeaponResult

		private List<WeaponResult> results = new List<WeaponResult>();
		[CategoryAttribute("Results"),
		DisplayName("Weapon Results")]
		public List<WeaponResult> Results
		{
			get { return results; }
			set { results = value; }
		}

		//==================================================================== Accuracy

		private string objecttypeaccuracy = "";
		[CategoryAttribute("Accuracy"),
		Browsable(false),
		DisplayName("Object Type")]
		public string Objecttypeaccuracy
		{
			get { return objecttypeaccuracy; }
			set { objecttypeaccuracy = value; }
		}

		private bool enabled = false;
		[CategoryAttribute("Accuracy"),
		DisplayName("Enabled")]
		public bool Enabled
		{
			get { return enabled; }
			set { enabled = value; }
		}

		private List<AccuracyProfile> accuracyprofiles = new List<AccuracyProfile>();
		[CategoryAttribute("Accuracy"),
		DisplayName("Accuracy Profiles")]
		public List<AccuracyProfile> Accuracyprofiles
		{
			get { return accuracyprofiles; }
			set { accuracyprofiles = value; }
		}

		//==================================================================== Penetration

		private string objecttypepenetration = "";
		[CategoryAttribute("Penetration"),
		Browsable(false),
		DisplayName("Object Type")]
		public string Objecttypepenetration
		{
			get { return objecttypepenetration; }
			set { objecttypepenetration = value; }
		}

		private int fieldpenetration = 0;
		[CategoryAttribute("Penetration"),
		DisplayName("Defense Field Penetration Chance")]
		public int Fieldpenetration
		{
			get { return fieldpenetration; }
			set { fieldpenetration = value; }
		}

		private float defaultpenetration = 0;
		[CategoryAttribute("Penetration"),
		DisplayName("Default Penetration")]
		public float Defaultpenetration
		{
			get { return defaultpenetration; }
			set { defaultpenetration = value; }
		}

		private List<PenetrationProfile> penetrationprofiles = new List<PenetrationProfile>();
		[CategoryAttribute("Penetration"),
		DisplayName("Penetration Profiles")]
		public List<PenetrationProfile> Penetrationprofiles
		{
			get { return penetrationprofiles; }
			set { penetrationprofiles = value; }
		}

		#endregion

		#region Misc and Sound
		//==================================================================== MiscValues

		private string objecttypemisc = "";
		[CategoryAttribute("Misc"),
		Browsable(false),
		DisplayName("Object Type")]
		public string Objecttypemisc
		{
			get { return objecttypemisc; }
			set { objecttypemisc = value; }
		}

		private float recoildistance = 0;
		[CategoryAttribute("Misc"),
		DisplayName("Recoil Distance")]
		public float Recoildistance
		{
			get { return recoildistance; }
			set { recoildistance = value; }
		}

		private float slavefiredelay = 0;
		[CategoryAttribute("Misc"),
		DisplayName("Slave Fire Delay")]
		public float Slavefiredelay
		{
			get { return slavefiredelay; }
			set { slavefiredelay = value; }
		}

		//==================================================================== AnimTurretSound

		private string objecttypesound = "";
		[CategoryAttribute("Sound"),
		Browsable(false),
		DisplayName("Object Type")]
		public string Objecttypesound
		{
			get { return objecttypesound; }
			set { objecttypesound = value; }
		}

		private string soundpath = "";
		[CategoryAttribute("Sound"),
		DisplayName("Sound Path")]
		public string Soundpath
		{
			get { return soundpath; }
			set { soundpath = value; }
		}

		#endregion

		#endregion

		#region Parsers

		private string[] ParseSetup(string source)
		{
			int first = source.IndexOf('(');
			int last = source.IndexOf(')');
			string data = source.Substring(first + 1, last - first - 1);

			string[] commaData = data.Split(new char[] { '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);			

			//Lazy removal of quotes
			for (int i = 0; i < commaData.Length; i++)
			{
				string entry = commaData[i];
				if (entry.Contains("\""))
				{
					entry = entry.Substring(1, entry.Length - 2);
				}
				commaData[i] = entry;
			}

			return commaData;
		}

		public void ParseAndLoadConfig(string source)
		{
			string[] data = ParseSetup(source);

			int counter = 0;

			weaponname = AttributeUtils.Assign<string>(data[counter++]);
			weapontype = AttributeUtils.Assign<string>(data[counter++]);
			weaponfiretype = AttributeUtils.Assign<string>(data[counter++]);
			weaponfirename = AttributeUtils.Assign<string>(data[counter++]);
			activation = AttributeUtils.Assign<string>(data[counter++]);
			weaponfirespeed = AttributeUtils.Assign<float>(data[counter++]);
			weaponfirerange = AttributeUtils.Assign<float>(data[counter++]);
			weaponfireradius = AttributeUtils.Assign<float>(data[counter++]);
			weaponfirelifetime = AttributeUtils.Assign<float>(data[counter++]);
			weaponfiremisc1 = AttributeUtils.Assign<float>(data[counter++]);
			weaponfireaxis = AttributeUtils.Assign<int>(data[counter++]);
			maxeffectsspawned = AttributeUtils.Assign<int>(data[counter++]);
			usevelocitypred = AttributeUtils.Assign<bool>(data[counter++]);
			checklineoffire = AttributeUtils.Assign<bool>(data[counter++]);
			firetime = AttributeUtils.Assign<float>(data[counter++]);
			burstfiretime = AttributeUtils.Assign<float>(data[counter++]);
			burstwaittime = AttributeUtils.Assign<float>(data[counter++]);
			shootatsecondaries = AttributeUtils.Assign<bool>(data[counter++]);
			shootatsurroundings = AttributeUtils.Assign<bool>(data[counter++]);
			maxazimuthspeed = AttributeUtils.Assign<float>(data[counter++]);
			maxdeclinationspeed = AttributeUtils.Assign<float>(data[counter++]);
			speedmultiplierwhenpointingattarget = AttributeUtils.Assign<float>(data[counter++]);
			weaponshieldpenetration = AttributeUtils.Assign<string>(data[counter++]);
			tracktargetsoutsiderange = AttributeUtils.Assign<bool>(data[counter++]);
			waituntillcoderedstate = AttributeUtils.Assign<bool>(data[counter++]);
			instanthitthreshold = AttributeUtils.Assign<int>(data[counter++]);
		}

		public void ParseAndLoadAngles(string source)
		{
			string[] data = ParseSetup(source);

			int counter = 0;

			objecttype = AttributeUtils.Assign<string>(data[counter++]);
			triggerhappy = AttributeUtils.Assign<float>(data[counter++]);
			minazimuth = AttributeUtils.Assign<float>(data[counter++]);
			maxazimuth = AttributeUtils.Assign<float>(data[counter++]);
			mindeclination = AttributeUtils.Assign<float>(data[counter++]);
			maxdeclination = AttributeUtils.Assign<float>(data[counter++]);
		}

		public void ParseAndLoadResult(string source)
		{
			WeaponResult result = new WeaponResult();
			result.ParseAndLoad(source);
			this.results.Add(result);
		}

		public void ParseAndLoadMisc(string source)
		{
			string[] data = ParseSetup(source);

			int counter = 0;

			objecttypemisc = AttributeUtils.Assign<string>(data[counter++]);
			recoildistance = AttributeUtils.Assign<float>(data[counter++]);
			slavefiredelay = AttributeUtils.Assign<float>(data[counter++]);
		}

		public void ParseAndLoadSound(string source)
		{
			string[] data = ParseSetup(source);

			int counter = 0;

			objecttypesound = AttributeUtils.Assign<string>(data[counter++]);
			soundpath = AttributeUtils.Assign<string>(data[counter++]);
		}

		public void ParseAndLoadAccuracy(string source)
		{
			string[] data = ParseSetup(source);

			int counter = 0;

			List<string> stuff = new List<string>(data);

			objecttypeaccuracy = AttributeUtils.Assign<string>(data[counter++]);
			enabled = AttributeUtils.Assign<bool>(data[counter++]);

			AccuracyProfile newProfile = null;
			bool waiting = false;
			for (;  counter < data.Length; counter++)
			{
				string[] parts = data[counter].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
				if(parts[0] == "damage")
				{
					newProfile.Alwaysdamage = Convert.ToBoolean(Convert.ToInt32(parts[1]));
					accuracyprofiles.Add(newProfile);
					newProfile = null;
					waiting = false;
				}
				else
				{
					if(newProfile == null)
					{
						newProfile = new AccuracyProfile();
						newProfile.Familyname = parts[0];
						newProfile.Value = Convert.ToSingle(parts[1]);
						waiting = true;
					}
					else
					{
						accuracyprofiles.Add(newProfile); 
						newProfile = new AccuracyProfile();
						newProfile.Familyname = parts[0];
						newProfile.Value = Convert.ToSingle(parts[1]);
						waiting = true;
					}
				}
			}

			if (waiting)
			{
				accuracyprofiles.Add(newProfile);
			}
		}

		public void ParseAndLoadPenetration(string source)
		{
			string[] data = ParseSetup(source);

			int counter = 0;

			List<string> stuff = new List<string>(data);

			objecttypepenetration = AttributeUtils.Assign<string>(data[counter++]);
			fieldpenetration = AttributeUtils.Assign<int>(data[counter++]);
			defaultpenetration = AttributeUtils.Assign<float>(data[counter++]);

			PenetrationProfile newProfile = null;
			for (; counter < data.Length; counter++)
			{
				string[] parts = data[counter].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

				newProfile = new PenetrationProfile();
				newProfile.Armorfamilyname = parts[0];
				newProfile.Value = Convert.ToSingle(parts[1]);
				penetrationprofiles.Add(newProfile);
			}
		}

		#endregion

		public event EventHandler AngleUpdate;

		public override string ToString()
		{
			string returnString = "";
			int pad = 100;

			returnString += 
				"--==================================\r\n" + 
				"--  Generated by Xerc's WeaponEd\r\n" + 
				"--==================================\r\n\r\n";

			returnString += "StartWeaponConfig(";
			returnString += this.weaponname + ",";
			returnString += "\"" + this.weapontype + "\",";
			returnString += "\"" + this.weaponfiretype + "\",";
			returnString += "\"" + this.weaponfirename + "\",";
			returnString += "\"" + this.activation + "\",";
			returnString += this.weaponfirespeed + ",";
			returnString += this.weaponfirerange + ",";
			returnString += this.weaponfireradius + ",";
			returnString += this.weaponfirelifetime + ",";
			returnString += this.weaponfiremisc1 + ",";
			returnString += this.weaponfireaxis + ",";
			returnString += this.maxeffectsspawned + ",";
			returnString += Convert.ToInt32(this.usevelocitypred) + ",";
			returnString += Convert.ToInt32(this.checklineoffire) + ",";
			returnString += this.firetime + ",";
			returnString += this.burstfiretime + ",";
			returnString += this.burstwaittime + ",";
			returnString += Convert.ToInt32(this.shootatsecondaries) + ",";
			returnString += Convert.ToInt32(this.shootatsurroundings) + ",";
			returnString += this.maxazimuthspeed + ",";
			returnString += this.maxdeclinationspeed + ",";
			returnString += this.speedmultiplierwhenpointingattarget + ",";
			returnString += "\"" + this.weaponshieldpenetration + "\",";
			returnString += Convert.ToInt32(this.tracktargetsoutsiderange) + ",";
			returnString += Convert.ToInt32(this.waituntillcoderedstate) + ",";
			returnString += this.instanthitthreshold + ");\r\n";

			if (soundpath != "")
			{
				returnString += "\r\n--Sound".PadRight(pad, '=') + "\r\n\r\n";
				returnString += "addAnimTurretSound(";
				returnString += this.objecttypesound + ",";
				returnString += "\"" + this.soundpath + "\");\r\n";
			}

			returnString += "\r\n--Weapon Results".PadRight(pad, '=') + "\r\n\r\n";

			foreach (WeaponResult result in this.results)
			{
				returnString += "AddWeaponResult(";
				returnString += result.Weapon + ",";
				returnString += "\"" + result.Condition + "\",";
				returnString += "\"" + result.Effect + "\",";
				returnString += "\"" + result.Target + "\",";
				returnString += result.Minimumeffect + ",";
				returnString += result.Maximumeffect + ",";
				returnString += "\"" + result.Spawnedweaponeffect + "\");\r\n";
			}

			returnString += "\r\n--Weapon Angles".PadRight(pad, '=') + "\r\n\r\n";

			returnString += "setAngles(";
			returnString += this.objecttype + ",";
			returnString += this.triggerhappy + ",";
			returnString += this.minazimuth + ",";
			returnString += this.maxazimuth + ",";
			returnString += this.mindeclination + ",";
			returnString += this.maxdeclination + ");\r\n";

			if (recoildistance != 0 || slavefiredelay != 0)
			{
				returnString += "\r\n--Recoil and Slave Fire Delay".PadRight(pad, '=') + "\r\n\r\n";

				returnString += "setMiscValues(";
				returnString += this.objecttypemisc + ",";
				returnString += this.recoildistance + ",";
				returnString += this.slavefiredelay + ");\r\n";
			}

			if (this.accuracyprofiles.Count > 0)
			{
				returnString += "\r\n--Weapon Accuracy".PadRight(pad, '=') + "\r\n\r\n";

				returnString += "setAccuracy(";
				returnString += this.objecttypeaccuracy + ",";
				returnString += Convert.ToInt32(this.enabled);
				foreach (AccuracyProfile prof in this.accuracyprofiles)
				{
					returnString += ",\r\n";
					returnString += "{" + prof.Familyname + "=" + prof.Value;
					if (prof.Alwaysdamage)
					{
						returnString += ",damage=1}";
					}
					else
					{
						returnString += "}";
					}
				}
				returnString += ");\r\n";
			}


				returnString += "\r\n--Weapon Penetration".PadRight(pad, '=') + "\r\n\r\n";

				returnString += "setPenetration(";
				returnString += this.objecttypepenetration + ",";
				returnString += this.fieldpenetration + ",";
				returnString += this.defaultpenetration;
				if (this.penetrationprofiles.Count > 0)
				{
					foreach (PenetrationProfile prof in this.penetrationprofiles)
					{
						returnString += ",\r\n";
						returnString += "{" + prof.Armorfamilyname + "=" + prof.Value + "}";
					}
					returnString += ");";
				}
				else
				{
					returnString += ");";
				}

				return returnString;
		}
	}

	public static class FamilyListReader
	{
		private static string familyLuaPath;
		public static string FamilyLuaPath
		{
			get { return FamilyListReader.familyLuaPath; }
			set 
			{ 
				FamilyListReader.familyLuaPath = value; 
				if(FamilyListReader.FamilyLuaPathChanged != null)
				{
					FamilyListReader.FamilyLuaPathChanged(null, EventArgs.Empty);
				}
			}
		}

		private static string weaponFire;
		public static string WeaponFire
		{
			get { return FamilyListReader.weaponFire; }
			set
			{ 
				FamilyListReader.weaponFire = value; 
				if(WeaponFirePathChanged != null)
				{
					WeaponFirePathChanged(null, EventArgs.Empty);
				}
			}
		}

		public static string[] GetAttackFamilies()
		{
			if(!File.Exists(familyLuaPath))
			{
				MessageBox.Show("The selected FamilyList.lua doesnt exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return new string[] { " " };
			}

			StreamReader reader = new StreamReader(familyLuaPath);
			bool functionStarted = false;
			string functionName = "";
			string functionLine = "";
			bool oneCurlyFound = false;

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();

				if (line.Contains("--"))
				{
					line = line.Substring(0, line.IndexOf("--"));
				}

				if (line.Contains("attackFamily ="))
				{
					functionStarted = true;
					functionName = line.Substring(0, line.LastIndexOf('='));
					functionLine = "";
				}

				if (line.Contains("}") && functionStarted && oneCurlyFound)
				{
					functionLine += line;
					break;
				}
				else if (functionStarted && line.Contains("}") && !oneCurlyFound)
				{
					functionLine += line;
					oneCurlyFound = true;
				}
				else if(functionStarted && line.Contains("{") && oneCurlyFound)
				{
					oneCurlyFound = false;
				}
				else if(functionStarted)
				{
					functionLine += line;
				}
			}

			reader.Close();

			string[] data = functionLine.Split(new char[] { '{', ',', '}'}, StringSplitOptions.RemoveEmptyEntries);
			List<string> names = new List<string>();
			foreach (string entry in data)
			{
				if(entry.Contains("name"))
				{
					int first  = entry.IndexOf('\"');
					int last = entry.LastIndexOf('\"');
					string name = entry.Substring(first + 1, (last - first) - 1);
					names.Add(name);
				}
			}

			return names.ToArray();
		}

		public static string[] GetArmorFamilies()
		{
			if (!File.Exists(familyLuaPath))
			{
				MessageBox.Show("The selected FamilyList.lua doesnt exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return new string[] { " " };
			}

			StreamReader reader = new StreamReader(familyLuaPath);
			bool functionStarted = false;
			string functionName = "";
			string functionLine = "";
			bool oneCurlyFound = false;

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();

				if (line.Contains("--"))
				{
					line = line.Substring(0, line.IndexOf("--"));
				}

				if (line.Contains("armourFamily ="))
				{
					functionStarted = true;
					functionName = line.Substring(0, line.LastIndexOf('='));
					functionLine = "";
				}

				if (line.Contains("}") && functionStarted && oneCurlyFound)
				{
					functionLine += line;
					break;
				}
				else if (functionStarted && line.Contains("}") && !oneCurlyFound)
				{
					functionLine += line;
					oneCurlyFound = true;
				}
				else if (functionStarted && line.Contains("{") && oneCurlyFound)
				{
					oneCurlyFound = false;
				}
				else if (functionStarted)
				{
					functionLine += line;
				}
			}

			reader.Close();

			string[] data = functionLine.Split(new char[] { '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);
			List<string> names = new List<string>();
			foreach (string entry in data)
			{
				if (entry.Contains("name"))
				{
					int first = entry.IndexOf('\"');
					int last = entry.LastIndexOf('\"');
					string name = entry.Substring(first + 1, (last - first) - 1);
					names.Add(name);
				}
			}

			return names.ToArray();
		}

		public static string[] GetWeaponFireEffects()
		{
			if (!File.Exists(weaponFire))
			{
				string[] list = Directory.GetFiles(weaponFire, "*.wf", SearchOption.AllDirectories);
				List<string> names = new List<string>();
				foreach (string item in list)
				{
					names.Add(Path.GetFileNameWithoutExtension(item));
				}
				return names.ToArray();
			}
			return new string[] { "" };
		}

		public static event EventHandler FamilyLuaPathChanged;
		public static event EventHandler WeaponFirePathChanged;
	}

	public class WeaponTypeConverter : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			string name = context.PropertyDescriptor.Name;
			StandardValuesCollection returnCollection;

			switch (name)
			{
				case "Weapontype":
					returnCollection =
						new StandardValuesCollection(new string[] {
						"Gimble",
						"AnimatedTurret",
						"Fixed"
						});
					break;
				case "Weaponfiretype":
					returnCollection =
						new StandardValuesCollection(new string[] {
						"InstantHit",
						"Bullet",
						"Mine",
						"Missile",
						"SphereBurst"
						});
					break;
				case "Activation":
					returnCollection =
						new StandardValuesCollection(new string[] {
						"Normal",
						"Special Attack",
						"Normal Only",
						"Dropped"
						});
					break;
				case "Weaponshieldpenetration":
					returnCollection =
						new StandardValuesCollection(new string[] {
						"Normal",
						"Enhanced",
						"Bypass"
						});
					break;
				case "Condition":
					returnCollection =
						new StandardValuesCollection(new string[] {
						"Hit",
						"Miss"
						});
					break;
				case "Effect":
					returnCollection =
						new StandardValuesCollection(new string[] {
						"DamageHealth",
						"Disable",
						"Push",
						"SpawnWeaponFire"
						});
					break;
				case "Target":
					returnCollection =
						new StandardValuesCollection(new string[] {
						"Target",
						"Owner"
						});
					break;
				case "Familyname":
					returnCollection =
						new StandardValuesCollection(
							FamilyListReader.GetAttackFamilies());
					break;
				case "Armorfamilyname":
					returnCollection =
						new StandardValuesCollection(
							FamilyListReader.GetArmorFamilies());
					break;
				case "Weaponfirename":
					returnCollection =
						new StandardValuesCollection(
							FamilyListReader.GetWeaponFireEffects());
					break;
				default:
					returnCollection =
						new StandardValuesCollection(new string[] {
						""
						});
					break;
			}

			return returnCollection;
		}
	}

	public class CheckBoxUIProp: UITypeEditor
	{
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			bool flag = Convert.ToBoolean(e.Value);
			ControlPaint.DrawCheckBox(e.Graphics, e.Bounds, (flag ? ButtonState.Checked : ButtonState.Normal));
		}
	}

	public class NumericUIProp : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService editorService = null;
			if (provider != null)
			{
				editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
			}
			if (editorService != null)
			{
				NumericUpDown udControl = new NumericUpDown();
				udControl.DecimalPlaces = 0;
				udControl.Minimum = -360;
				udControl.Maximum = 360;
				udControl.Value = Convert.ToDecimal(value);
				editorService.DropDownControl(udControl);
				value = Convert.ToDecimal(udControl.Value);
			}

			return value;
		}
	}

	public class UpdatePanel : System.Windows.Forms.Panel
	{
		public UpdatePanel()
        {
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint | 
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint | 
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, 
                true);
        }
	}
}