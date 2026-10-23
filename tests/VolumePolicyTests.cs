using System;
using System.IO;

namespace JblVolume {
 static class VolumePolicyTests {
  static int checks;
  static void Equal(float expected,float actual,string message) {
   checks++;
   if(Math.Abs(expected-actual)>0.00001f)throw new Exception(message+": expected "+expected+", got "+actual);
  }
  static void True(bool value,string message) { checks++;if(!value)throw new Exception(message); }
  static int Main() {
   string folder=Path.Combine(Path.GetTempPath(),"JBLVolume-regression-"+Guid.NewGuid().ToString("N"));
   Directory.CreateDirectory(folder);
   try {
    string path=Path.Combine(folder,"preferences.tsv");
    var policy=new VolumePolicy(path);
    // Exact reported failure: Windows restores a 20% session after reboot.
    Equal(0.47f,policy.Target("boot1","zen",false,0.2f,0.47f,false,0.05f,true),"restored mixer level must not become a 20% cap");
    policy.Applied("boot1",0.47f,0.47f,false);
    Equal(1,policy.Target("boot1","zen",false,0.47f,1,false,0.05f,true),"full master reaches full session volume");
    policy.Applied("boot1",1,1,false);
    Equal(0,policy.Target("boot1","zen",false,1,0,false,0.05f,true),"zero master is silent");
    policy.Applied("boot1",0,0,false);
    Equal(0.4f,policy.Target("boot1","zen",false,0,0.4f,false,0.05f,true),"returning from zero retains full range");
    policy.Applied("boot1",0.4f,0.4f,false);
    Equal(0,policy.Target("boot1","zen",false,0.4f,0.4f,true,0.05f,true),"mute is silent");
    policy.Applied("boot1",0,0.4f,true);
    Equal(0.4f,policy.Target("boot1","zen",false,0,0.4f,false,0.05f,true),"unmute restores the prior gain");
    policy.Applied("boot1",0.4f,0.4f,false);
    // A deliberate mixer edit to 0.2 at a previous master of 0.4 means 50% trim.
    // Master changes simultaneously: normalize by the PREVIOUS applied master.
    Equal(0.4f,policy.Target("boot1","zen",false,0.2f,0.8f,false,0.05f,true),"manual edit uses previous factor");
    policy.Applied("boot1",0.4f,0.8f,false);
    var rebooted=new VolumePolicy(path);
    Equal(0.4f,rebooted.Target("boot2","zen",false,0.4f,0.8f,false,0.05f,true),"relative preference survives a new process without compounding");
    rebooted.Applied("boot2",0.4f,0.8f,false);
    Equal(0.5f,rebooted.Target("boot2","zen",false,0.4f,1,false,0.05f,true),"intentional trim is preserved");
    rebooted.Applied("boot2",0.5f,1,false);
    var anotherRestart=new VolumePolicy(path);
    Equal(0.5f,anotherRestart.Target("boot3","zen",false,0.5f,1,false,0.05f,true),"repeated restart does not multiply the trim again");
    Equal(0.025f,anotherRestart.Target("system","sounds",true,1,0.5f,false,0.05f,true),"system sounds retain their separate attenuation");
    Equal(0,anotherRestart.Target("system","sounds",true,1,0.5f,true,0.05f,true),"mute also silences system sounds");
    Equal(0.5f,anotherRestart.Target("new","other",false,0.094f,0.5f,false,0.05f,true),"new applications do not inherit already attenuated outputs");
    string before=File.ReadAllText(path);
    Equal(0.35f,anotherRestart.Target("diagnose","zen",false,0.01f,0.7f,false,0.05f,false),"diagnostics do not infer a new trim");
    True(before==File.ReadAllText(path),"diagnostics do not modify preferences");
    Equal(0.01f,anotherRestart.Target("low","fresh",false,0,0.01f,false,0.05f,true),"low end of full range works");
    policy.Save();
    Console.WriteLine("PASS: "+checks+" regression checks (restart, full range, mixer edits, mute, system sounds, read-only diagnostics).");
    return 0;
   } finally {
    // Remove only the exact files this test created in its unique directory.
    foreach(string file in Directory.GetFiles(folder))File.Delete(file);
    Directory.Delete(folder);
   }
  }
 }
}
