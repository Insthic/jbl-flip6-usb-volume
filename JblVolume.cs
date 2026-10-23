using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace JblVolume {
 [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")] class DeviceEnumerator {}
 [ComImport, Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] interface IDeviceEnumerator {
  [PreserveSig] int EnumAudioEndpoints(int flow, uint mask, out IDeviceCollection devices);
  [PreserveSig] int GetDefaultAudioEndpoint(int flow, int role, out IDevice device);
  [PreserveSig] int GetDevice([MarshalAs(UnmanagedType.LPWStr)] string id, out IDevice device);
 }
 [ComImport, Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] interface IDeviceCollection {
  [PreserveSig] int GetCount(out uint count);
  [PreserveSig] int Item(uint index, out IDevice device);
 }
 [ComImport, Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] interface IDevice {
  [PreserveSig] int Activate(ref Guid iid, uint context, IntPtr parameters, [MarshalAs(UnmanagedType.IUnknown)] out object result);
  [PreserveSig] int OpenPropertyStore(uint access, out IPropertyStore store);
  [PreserveSig] int GetId([MarshalAs(UnmanagedType.LPWStr)] out string id);
  [PreserveSig] int GetState(out uint state);
 }
 [StructLayout(LayoutKind.Sequential)] struct PropertyKey { public Guid fmtid; public uint pid; public PropertyKey(string g,uint p){fmtid=new Guid(g);pid=p;} }
 [StructLayout(LayoutKind.Explicit, Size=24)] struct PropVariant { [FieldOffset(0)] public ushort type; [FieldOffset(8)] public IntPtr pointer; }
 [ComImport, Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] interface IPropertyStore {
  [PreserveSig] int GetCount(out uint count);
  [PreserveSig] int GetAt(uint index,out PropertyKey key);
  [PreserveSig] int GetValue(ref PropertyKey key,out PropVariant value);
 }
 [ComImport, Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] interface IEndpointVolume {
  [PreserveSig] int RegisterControlChangeNotify(IntPtr callback);
  [PreserveSig] int UnregisterControlChangeNotify(IntPtr callback);
  [PreserveSig] int GetChannelCount(out uint count);
  [PreserveSig] int SetMasterVolumeLevel(float level,ref Guid context);
  [PreserveSig] int SetMasterVolumeLevelScalar(float level,ref Guid context);
  [PreserveSig] int GetMasterVolumeLevel(out float level);
  [PreserveSig] int GetMasterVolumeLevelScalar(out float level);
  [PreserveSig] int SetChannelVolumeLevel(uint channel,float level,ref Guid context);
  [PreserveSig] int SetChannelVolumeLevelScalar(uint channel,float level,ref Guid context);
  [PreserveSig] int GetChannelVolumeLevel(uint channel,out float level);
  [PreserveSig] int GetChannelVolumeLevelScalar(uint channel,out float level);
  [PreserveSig] int SetMute([MarshalAs(UnmanagedType.Bool)] bool mute,ref Guid context);
  [PreserveSig] int GetMute([MarshalAs(UnmanagedType.Bool)] out bool mute);
  [PreserveSig] int GetVolumeStepInfo(out uint step,out uint count);
  [PreserveSig] int VolumeStepUp(ref Guid context);
  [PreserveSig] int VolumeStepDown(ref Guid context);
  [PreserveSig] int QueryHardwareSupport(out uint mask);
 }
 [ComImport, Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] interface ISessionManager {
  [PreserveSig] int GetAudioSessionControl(IntPtr session,uint flags,out IntPtr control);
  [PreserveSig] int GetSimpleAudioVolume(IntPtr session,uint flags,out ISimpleVolume volume);
  [PreserveSig] int GetSessionEnumerator(out ISessionEnumerator sessions);
 }
 [ComImport, Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] interface ISessionEnumerator {
  [PreserveSig] int GetCount(out int count);
  [PreserveSig] int GetSession(int index,out ISessionControl session);
 }
 [ComImport, Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] interface ISessionControl {
  [PreserveSig] int GetState(out int state);
  [PreserveSig] int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string name);
  [PreserveSig] int SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string name,ref Guid context);
  [PreserveSig] int GetIconPath([MarshalAs(UnmanagedType.LPWStr)] out string path);
  [PreserveSig] int SetIconPath([MarshalAs(UnmanagedType.LPWStr)] string path,ref Guid context);
  [PreserveSig] int GetGroupingParam(out Guid group);
  [PreserveSig] int SetGroupingParam(ref Guid group,ref Guid context);
  [PreserveSig] int RegisterAudioSessionNotification(IntPtr events);
  [PreserveSig] int UnregisterAudioSessionNotification(IntPtr events);
  [PreserveSig] int GetSessionIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string id);
  [PreserveSig] int GetSessionInstanceIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string id);
  [PreserveSig] int GetProcessId(out uint pid);
 }
 [ComImport, Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)] interface ISimpleVolume {
  [PreserveSig] int SetMasterVolume(float volume,ref Guid context);
  [PreserveSig] int GetMasterVolume(out float volume);
  [PreserveSig] int SetMute([MarshalAs(UnmanagedType.Bool)] bool mute,ref Guid context);
  [PreserveSig] int GetMute([MarshalAs(UnmanagedType.Bool)] out bool mute);
 }
 class SessionLevel { public float Last; public float Factor; }
 class VolumePolicy {
  readonly string path;
  readonly Dictionary<string,float> trims=new Dictionary<string,float>(StringComparer.Ordinal);
  readonly Dictionary<string,SessionLevel> observed=new Dictionary<string,SessionLevel>(StringComparer.Ordinal);
  public bool HasSavedState { get { return File.Exists(path); } }
  public VolumePolicy(string statePath) {
   path=statePath;
   if(!File.Exists(path))return;
   string[] lines=File.ReadAllLines(path);
   if(lines.Length==0||lines[0]!="# JBLVolume relative levels v2")throw new InvalidDataException("Invalid volume preferences file.");
   for(int i=1;i<lines.Length;i++) {
    string[] parts=lines[i].Split('\t');float trim;
    if(parts.Length!=2||!float.TryParse(parts[1],NumberStyles.Float,CultureInfo.InvariantCulture,out trim)||float.IsNaN(trim)||trim<0||trim>1)continue;
    try{trims[System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(parts[0]))]=trim;}catch(FormatException){}
   }
  }
  public void Save() {
   var lines=new List<string>{"# JBLVolume relative levels v2"};
   foreach(var entry in trims)lines.Add(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(entry.Key))+"\t"+entry.Value.ToString("R",CultureInfo.InvariantCulture));
   string temporary=path+".tmp";File.WriteAllLines(temporary,lines);
   if(File.Exists(path))File.Replace(temporary,path,null);else File.Move(temporary,path);
  }
  public float Target(string instance,string application,bool system,float current,float master,bool muted,float systemGain,bool apply) {
   float trim;if(!trims.TryGetValue(application,out trim))trim=1;
   SessionLevel last;
   // A restored Windows level is an OUTPUT, never a new relative preference.
   // Only a change observed after we applied a level counts as a mixer edit.
   if(apply&&!system&&observed.TryGetValue(instance,out last)&&last.Factor>0.0001f&&Math.Abs(current-last.Last)>0.002f) {
    trim=Math.Max(0,Math.Min(1,current/last.Factor));
    trims[application]=trim;Save();
   }
   return muted?0:Math.Max(0,Math.Min(1,master*(system?systemGain:trim)));
  }
  public void Applied(string instance,float actual,float master,bool muted) {
   observed[instance]=new SessionLevel{Last=actual,Factor=muted?0:master};
  }
 }
 class Audio : IDisposable {
  static Guid context=new Guid("50754d6e-7a54-4708-a81a-5cfe9c217e64");
  IDeviceEnumerator enumerator; IDevice device; IEndpointVolume endpoint; ISessionManager manager;
  VolumePolicy policy;
  public string Name="JBL / Zgmicro"; public float Master; public bool Muted; public int Sessions;
  public string Id; public uint HardwareSupport; public float SystemSoundsGain=0.05f;
  [DllImport("ole32.dll")] static extern int PropVariantClear(ref PropVariant value);
  public static void Check(int hr){Marshal.ThrowExceptionForHR(hr);}
  public static void Release(object o){if(o!=null && Marshal.IsComObject(o)) Marshal.ReleaseComObject(o);}
  static string FriendlyName(IDevice d){IPropertyStore store=null; PropVariant v=new PropVariant();try{Check(d.OpenPropertyStore(0,out store));var key=new PropertyKey("a45c254e-df1c-4efd-8020-67d146a850e0",14);Check(store.GetValue(ref key,out v));return v.type==31?Marshal.PtrToStringUni(v.pointer):"";}finally{PropVariantClear(ref v);Release(store);}}
  public Audio(){
   try {
    enumerator=(IDeviceEnumerator)new DeviceEnumerator(); IDeviceCollection devices=null;
    try{Check(enumerator.EnumAudioEndpoints(0,1,out devices));uint count;Check(devices.GetCount(out count));
     for(uint i=0;i<count;i++){IDevice candidate=null;Check(devices.Item(i,out candidate));string id;Check(candidate.GetId(out id));string name=FriendlyName(candidate);
      if(name.IndexOf("Zgmicro",StringComparison.OrdinalIgnoreCase)>=0){if(device!=null){Release(candidate);throw new InvalidOperationException("Multiple Zgmicro devices are connected. Device selection is required.");}device=candidate;Id=id;Name=name;}else Release(candidate);
     }
    }finally{Release(devices);}
    if(device==null)throw new InvalidOperationException("JBL / Zgmicro is not connected. Waiting for USB...");
    Guid iid=typeof(IEndpointVolume).GUID;object result;Check(device.Activate(ref iid,23,IntPtr.Zero,out result));endpoint=(IEndpointVolume)result;
    iid=typeof(ISessionManager).GUID;Check(device.Activate(ref iid,23,IntPtr.Zero,out result));manager=(ISessionManager)result;
    Check(endpoint.QueryHardwareSupport(out HardwareSupport));policy=new VolumePolicy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"application-levels-v2.tsv"));Read();
   }catch{Dispose();throw;}
  }
  public void Read(){Check(endpoint.GetMasterVolumeLevelScalar(out Master));Check(endpoint.GetMute(out Muted));uint state;Check(device.GetState(out state));if(state!=1)throw new InvalidOperationException("JBL disconnected");}
  public void Set(float scalar){Check(endpoint.SetMasterVolumeLevelScalar(Math.Max(0,Math.Min(1,scalar)),ref context));}
  public void ToggleMute(){Read();Check(endpoint.SetMute(!Muted,ref context));}
  void MigrateLegacyLevels() {
   if(policy.HasSavedState)return;
   // The v1 file is intentionally not imported: its saved baselines may already
   // contain compounded attenuation. Keep the file for recovery instead.
   if(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"session-levels.tsv"))) {
    float safe=Master;ISessionEnumerator sessions=null;
    try{Check(manager.GetSessionEnumerator(out sessions));int count;Check(sessions.GetCount(out count));
     for(int i=0;i<count;i++){ISessionControl control=null;try{Check(sessions.GetSession(i,out control));int state;uint pid;Check(control.GetState(out state));Check(control.GetProcessId(out pid));
      if(state==1&&pid!=0){float current;Check(((ISimpleVolume)control).GetMasterVolume(out current));safe=Math.Min(safe,current);}
     }finally{Release(control);}}
    }finally{Release(sessions);}
    if(safe<Master){Set(safe);Read();}
   }
   policy.Save();
  }
  public List<string> Sync(bool apply){
   Read();if(apply)MigrateLegacyLevels();ISessionEnumerator sessions=null;Sessions=0;var rows=new List<string>();
   try{Check(manager.GetSessionEnumerator(out sessions));int count;Check(sessions.GetCount(out count));
    for(int i=0;i<count;i++){ISessionControl control=null;try{
     Check(sessions.GetSession(i,out control));int state;Check(control.GetState(out state));if(state==2)continue;
     string key,application;Check(control.GetSessionInstanceIdentifier(out key));Check(control.GetSessionIdentifier(out application));uint pid;Check(control.GetProcessId(out pid));
     ISimpleVolume volume=(ISimpleVolume)control;float current;Check(volume.GetMasterVolume(out current));
     float target=policy.Target(key,application,pid==0,current,Master,Muted,SystemSoundsGain,apply);
     if(apply && Math.Abs(current-target)>0.0001f){Check(volume.SetMasterVolume(target,ref context));float readback;Check(volume.GetMasterVolume(out readback));if(Math.Abs(readback-target)>0.002f)throw new InvalidOperationException("The audio session did not accept the volume adjustment");current=readback;}
     if(apply)policy.Applied(key,current,Master,Muted);Sessions++;
     rows.Add(string.Format(CultureInfo.InvariantCulture,"pid={0} state={1} session={2:0.0000} target={3:0.0000}",pid,state,current,target));
    }finally{Release(control);}}
   }finally{Release(sessions);}return rows;
  }
  public void Dispose(){Release(manager);manager=null;Release(endpoint);endpoint=null;Release(device);device=null;Release(enumerator);enumerator=null;}
 }
 class VolumeForm : Form {
  NotifyIcon tray; TrackBar slider; Label status,label; System.Windows.Forms.Timer timer; Audio audio;
  bool updating; int retries; string lastError=""; bool allowClose; float systemGain=0.05f;
  public VolumeForm(bool minimized){
   Text="JBL Flip 6 Volume";Size=new Size(430,220);FormBorderStyle=FormBorderStyle.FixedDialog;MaximizeBox=false;StartPosition=FormStartPosition.CenterScreen;
   label=new Label{Left=20,Top=18,Width=385,Height=28,Text="JBL Flip 6 - Zgmicro USB",Font=new Font("Segoe UI",12)};
   slider=new TrackBar{Left=18,Top=55,Width=380,Minimum=0,Maximum=100,TickFrequency=10};
   status=new Label{Left=20,Top=115,Width=380,Height=45,Text="Connecting..."};Controls.AddRange(new Control[]{label,slider,status});
   var menu=new ContextMenuStrip();menu.Items.Add("Open volume control",null,delegate{ShowWindow();});menu.Items.Add("Mute / unmute",null,delegate{try{if(audio!=null)audio.ToggleMute();}catch(Exception e){status.Text=e.Message;}});
   var startup=new ToolStripMenuItem("Start with Windows"){CheckOnClick=true,Checked=StartupEnabled()};startup.CheckedChanged+=delegate{SetStartup(startup.Checked);};menu.Items.Add(startup);
   var systemQuiet=new ToolStripMenuItem("Soften Windows system sounds"){CheckOnClick=true,Checked=true};systemQuiet.CheckedChanged+=delegate{systemGain=systemQuiet.Checked?0.05f:1f;if(audio!=null)audio.SystemSoundsGain=systemGain;};menu.Items.Add(systemQuiet);
   menu.Items.Add("Exit (keep current volume)",null,delegate{allowClose=true;Close();});
   tray=new NotifyIcon{Icon=SystemIcons.Information,Text="JBL Flip 6 Volume",Visible=true,ContextMenuStrip=menu};tray.DoubleClick+=delegate{ShowWindow();};
   slider.ValueChanged+=delegate{if(!updating && audio!=null){try{audio.Set(slider.Value/100f);Tick();}catch(Exception e){status.Text=e.Message;}}};
   timer=new System.Windows.Forms.Timer{Interval=30};timer.Tick+=delegate{Tick();};timer.Start();Tick();
   if(minimized){ShowInTaskbar=false;WindowState=FormWindowState.Minimized;Shown+=delegate{Hide();};}
   FormClosing+=delegate(object sender,FormClosingEventArgs e){if(!allowClose && e.CloseReason==CloseReason.UserClosing){e.Cancel=true;Hide();ShowInTaskbar=false;}};
  }
  void ShowWindow(){ShowInTaskbar=true;Show();WindowState=FormWindowState.Normal;Activate();}
  void Tick(){try{if(audio==null){if(retries++%70!=0)return;audio=new Audio();audio.SystemSoundsGain=systemGain;}audio.Sync(true);updating=true;slider.Value=Math.Max(0,Math.Min(100,(int)Math.Round(audio.Master*100)));updating=false;
    label.Text="JBL Flip 6 - "+slider.Value+" %";status.Text=(audio.Muted?"Muted":"Software volume control active")+" - "+audio.Sessions+" sessions\nUse Windows volume or your keyboard media keys.";tray.Text="JBL Flip 6: "+slider.Value+" %";lastError="";
   }catch(Exception e){updating=false;if(audio!=null){audio.Dispose();audio=null;}status.Text=e.Message;if(e.Message!=lastError){File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"errors.log"),DateTime.Now.ToString("s")+" "+e.Message+Environment.NewLine);lastError=e.Message;}}}
  static bool StartupEnabled(){using(var key=Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run")){return key!=null&&key.GetValue("JBLVolume")!=null;}}
  static void SetStartup(bool enabled){using(var key=Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run")){if(enabled)key.SetValue("JBLVolume","\""+Application.ExecutablePath+"\" --tray");else key.DeleteValue("JBLVolume",false);}}
  protected override void Dispose(bool disposing){if(disposing){if(timer!=null)timer.Dispose();if(tray!=null){tray.Visible=false;tray.Dispose();}if(audio!=null)audio.Dispose();}base.Dispose(disposing);}
 }
 static class Program {
  [STAThread] static int Main(string[] args){
   if(args.Length>0 && (args[0]=="--diagnose"||args[0]=="--apply-once")){
    string report=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"diagnostic.txt");
    try{using(var audio=new Audio()){var rows=audio.Sync(args[0]=="--apply-once");var text="Device="+audio.Name+"\nId="+audio.Id+"\nHardwareSupport="+audio.HardwareSupport+"\nMaster="+audio.Master.ToString(CultureInfo.InvariantCulture)+"\nMute="+audio.Muted+"\nSessions="+audio.Sessions+"\n"+string.Join("\n",rows);File.WriteAllText(report,text);return 0;}}
    catch(Exception e){File.WriteAllText(report,e.ToString());return 1;}
   }
   bool created;using(var mutex=new Mutex(true,"Local\\JBLVolume-Zgmicro",out created)){if(!created)return 0;Application.EnableVisualStyles();Application.SetCompatibleTextRenderingDefault(false);Application.Run(new VolumeForm(args.Length>0&&args[0]=="--tray"));}return 0;
  }
 }
}
