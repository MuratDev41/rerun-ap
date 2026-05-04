.assembly extern netstandard
{
  .ver 2:0:0:0
  .publickeytoken = (CC 7B 13 FF CD 2D DD 51 ) // .{...-.Q
}
.assembly extern UnityEngine.CoreModule
{
  .ver 0:0:0:0
}
.assembly extern UnityEngine.AudioModule
{
  .ver 0:0:0:0
}
.assembly extern UnityEngine.PhysicsModule
{
  .ver 0:0:0:0
}
.assembly extern UnityEngine.ParticleSystemModule
{
  .ver 0:0:0:0
}
.assembly extern UnityEngine.UnityWebRequestWWWModule
{
  .ver 0:0:0:0
}
.assembly extern Unity.TextMeshPro
{
  .ver 0:0:0:0
}
.assembly extern Unity.Postprocessing.Runtime
{
  .ver 0:0:0:0
}
.assembly extern UnityEngine.UI
{
  .ver 1:0:0:0
}
.assembly extern UnityEngine.AnimationModule
{
  .ver 0:0:0:0
}
.assembly extern UnityEngine.InputLegacyModule
{
  .ver 0:0:0:0
}
.assembly 'Assembly-CSharp'
{
  .custom instance void class [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::'.ctor'(int32) =  (01 00 08 00 00 00 00 00 ) // ........

  .custom instance void class [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::'.ctor'() =  (
		01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
		63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01       ) // ceptionThrows.

  .custom instance void class [mscorlib]System.Diagnostics.DebuggableAttribute::'.ctor'(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) =  (01 00 02 00 00 00 00 00 ) // ........

  .hash algorithm 0x00008004
  .ver  0:0:0:0
}
.module 'Assembly-CSharp.dll' // GUID = {088399FF-BF63-48AD-ADF4-F12623C3ADC8}


  .class public auto ansi beforefieldinit AlideAudio
  	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
  {
    .field  public  <BROKEN CLASS token_ 200003e due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> player
    .field  private  <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> sfx
    .field  public  <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> startSlideSfx

    // method line 1
    .method private hidebysig 
           instance default void Awake ()  cil managed 
    {
        // Method begins at RVA 0x2050
	// Code size 13 (0xd)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldarg.0 
	IL_0002:  call Could not decode method token 0x25 due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.
	IL_0007:  stfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> AlideAudio::sfx
	IL_000c:  ret 
    } // end of method AlideAudio::Awake

    // method line 2
    .method private hidebysig 
           instance default void Update ()  cil managed 
    {
        // Method begins at RVA 0x2060
    } // end of method AlideAudio::Update

    // method line 3
    .method public hidebysig 
           instance default void PlayStartSlide ()  cil managed 
    {
        // Method begins at RVA 0x20de
	// Code size 12 (0xc)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> AlideAudio::startSlideSfx
	IL_0006:  callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
	IL_000b:  ret 
    } // end of method AlideAudio::PlayStartSlide

    // method line 4
    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' ()  cil managed 
    {
        // Method begins at RVA 0x20eb
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call instance void [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::.ctor()
	IL_0006:  ret 
    } // end of method AlideAudio::.ctor

  } // end of class AlideAudio

  .class public auto ansi beforefieldinit Archer
  	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
  {

    // method line 5
    .method private hidebysig 
           instance default void Start ()  cil managed 
    {
        // Method begins at RVA 0x20f3
	// Code size 1 (0x1)
	.maxstack 8
	IL_0000:  ret 
    } // end of method Archer::Start

    // method line 6
    .method private hidebysig 
           instance default void Update ()  cil managed 
    {
        // Method begins at RVA 0x20f3
	// Code size 1 (0x1)
	.maxstack 8
	IL_0000:  ret 
    } // end of method Archer::Update

    // method line 7
    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' ()  cil managed 
    {
        // Method begins at RVA 0x20eb
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call instance void [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::.ctor()
	IL_0006:  ret 
    } // end of method Archer::.ctor

  } // end of class Archer

  .class public auto ansi beforefieldinit Arrow
  	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
  {
    .field  private  <BROKEN CLASS token_ 1000008 due to Could not load file or assembly 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> collider
    .field  private  <BROKEN CLASS token_ 1000009 due to Could not load file or assembly 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> rb
    .field  public  <BROKEN CLASS token_ 100000a due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> swordHit
    .field  public  <BROKEN CLASS token_ 100000a due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> blood
    .field  public  <BROKEN CLASS token_ 100000a due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> arrowHit
    .field  private  bool done

    // method line 8
    .method private hidebysig 
           instance default void Awake ()  cil managed 
    {
        // Method begins at RVA 0x20f5
	// Code size 53 (0x35)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldarg.0 
	IL_0002:  call Could not decode method token 0x25 due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.
	IL_0007:  stfld <BROKEN CLASS token_ 1000008 due to Could not load file or assembly 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Arrow::collider
	IL_000c:  ldarg.0 
	IL_000d:  ldarg.0 
	IL_000e:  call Could not decode method token 0x25 due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.
	IL_0013:  stfld <BROKEN CLASS token_ 1000009 due to Could not load file or assembly 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Arrow::rb
	IL_0018:  ldarg.0 
	IL_0019:  ldfld <BROKEN CLASS token_ 1000008 due to Could not load file or assembly 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Arrow::collider
	IL_001e:  ldc.i4.0 
	IL_001f:  callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
	IL_0024:  ldarg.0 
	IL_0025:  ldstr "ActivateCollider"
	IL_002a:  ldc.r4 0.25
	IL_002f:  call instance void [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::Invoke(string, float32)
	IL_0034:  ret 
    } // end of method Arrow::Awake

    // method line 9
    .method private hidebysig 
           instance default void ActivateCollider ()  cil managed 
    {
        // Method begins at RVA 0x212b
	// Code size 13 (0xd)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldfld <BROKEN CLASS token_ 1000008 due to Could not load file or assembly 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Arrow::collider
	IL_0006:  ldc.i4.1 
	IL_0007:  callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
	IL_000c:  ret 
    } // end of method Arrow::ActivateCollider

    // method line 10
    .method private hidebysig 
           instance default void Update ()  cil managed 
    {
        // Method begins at RVA 0x2139
	// Code size 55 (0x37)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call instance <BROKEN CLASS token_ 100001b due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
	IL_0006:  ldarg.0 
	IL_0007:  call instance <BROKEN CLASS token_ 100001b due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
	IL_000c:  callvirt instance <BROKEN CLASS token_ 100001f due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
	IL_0011:  ldarg.0 
	IL_0012:  ldfld <BROKEN CLASS token_ 1000009 due to Could not load file or assembly 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Arrow::rb
	IL_0017:  callvirt instance <BROKEN CLASS token_ 1000007 due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> [UnityEngine.PhysicsModule]UnityEngine.Rigidbody::get_velocity()
	IL_001c:  call <BROKEN CLASS token_ 100001f due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> [UnityEngine.CoreModule]UnityEngine.Quaternion::LookRotation(<BROKEN CLASS token_ 1000007 due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.>)
	IL_0021:  call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
	IL_0026:  ldc.r4 5.
	IL_002b:  mul 
	IL_002c:  call <BROKEN CLASS token_ 100001f due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> [UnityEngine.CoreModule]UnityEngine.Quaternion::Lerp(<BROKEN CLASS token_ 100001f due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.>, <BROKEN CLASS token_ 100001f due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.>, float32)
	IL_0031:  callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_rotation(<BROKEN CLASS token_ 100001f due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.>)
	IL_0036:  ret 
    } // end of method Arrow::Update

    // method line 11
    .method private hidebysig 
           (null) cil managed 
    {
        // Method begins at RVA 0x2174
    } // end of method Arrow::<NULL METHOD SIGNATURE>

    // method line 12
    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' ()  cil managed 
    {
        // Method begins at RVA 0x20eb
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call instance void [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::.ctor()
	IL_0006:  ret 
    } // end of method Arrow::.ctor

  } // end of class Arrow

  .class public auto ansi beforefieldinit Music
  	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
  {
    .field  private  <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> music
    .field  private  float32 fadeSpeed
    .field  private  float32 desiredVolume
    .field  private  <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> ambient
    .field  private  <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> intense
    .field  private  <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> battle
    .field  private static  <BROKEN CLASS token_ 2000005 due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> '<Instance>k__BackingField'
    .custom instance void class [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::'.ctor'() =  (01 00 00 00 ) // ....


    // method line 13
    .method public static hidebysig specialname 
           default class Music get_Instance ()  cil managed 
    {
        .custom instance void class [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::'.ctor'() =  (01 00 00 00 ) // ....

        // Method begins at RVA 0x2382
	// Code size 6 (0x6)
	.maxstack 8
	IL_0000:  ldsfld class Music Music::'<Instance>k__BackingField'
	IL_0005:  ret 
    } // end of method Music::get_Instance

    // method line 14
    .method private static hidebysig specialname 
           default void set_Instance (class Music 'value')  cil managed 
    {
        .custom instance void class [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::'.ctor'() =  (01 00 00 00 ) // ....

        // Method begins at RVA 0x2389
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  stsfld class Music Music::'<Instance>k__BackingField'
	IL_0006:  ret 
    } // end of method Music::set_Instance

    // method line 15
    .method private hidebysig 
           instance default void Awake ()  cil managed 
    {
        // Method begins at RVA 0x2394
	// Code size 80 (0x50)
	.maxstack 2
	IL_0000:  ldarg.0 
	IL_0001:  call void class Music::set_Instance(class Music)
	IL_0006:  ldarg.0 
	IL_0007:  ldarg.0 
	IL_0008:  call Could not decode method token 0x25 due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.
	IL_000d:  stfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_0012:  ldarg.0 
	IL_0013:  ldstr "Music/ambient"
	IL_0018:  call Could not decode method token 0x77 due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.
	IL_001d:  stfld <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::ambient
	IL_0022:  ldarg.0 
	IL_0023:  ldstr "Music/intense"
	IL_0028:  call Could not decode method token 0x77 due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.
	IL_002d:  stfld <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::intense
	IL_0032:  ldarg.0 
	IL_0033:  ldstr "Music/battle"
	IL_0038:  call Could not decode method token 0x77 due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.
	IL_003d:  stfld <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::battle
	IL_0042:  ldarg.0 
	IL_0043:  ldarg.0 
	IL_0044:  call instance class [mscorlib]System.Collections.IEnumerator class Music::loadMusic()
	IL_0049:  call instance <BROKEN CLASS token_ 100004e due to Could not load file or assembly 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
	IL_004e:  pop 
	IL_004f:  ret 
    } // end of method Music::Awake

    // method line 16
    .method public hidebysig 
           instance default void PlayMusic (valuetype Music/MusicType t, float32 fadeSpeed)  cil managed 
    {
        // Method begins at RVA 0x23f0
	// Code size 124 (0x7c)
	.maxstack 2
	IL_0000:  ldarg.0 
	IL_0001:  ldarg.2 
	IL_0002:  stfld float32 Music::fadeSpeed
	IL_0007:  ldarg.0 
	IL_0008:  ldc.r4 1.
	IL_000d:  stfld float32 Music::desiredVolume
	IL_0012:  ldarg.1 
	IL_0013:  switch (
	  IL_0025,
	  IL_0042,
	  IL_005f)
	IL_0024:  ret 
	IL_0025:  ldarg.0 
	IL_0026:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_002b:  ldarg.0 
	IL_002c:  ldfld <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::ambient
	IL_0031:  callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(<BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.>)
	IL_0036:  ldarg.0 
	IL_0037:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_003c:  callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
	IL_0041:  ret 
	IL_0042:  ldarg.0 
	IL_0043:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_0048:  ldarg.0 
	IL_0049:  ldfld <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::intense
	IL_004e:  callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(<BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.>)
	IL_0053:  ldarg.0 
	IL_0054:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_0059:  callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
	IL_005e:  ret 
	IL_005f:  ldarg.0 
	IL_0060:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_0065:  ldarg.0 
	IL_0066:  ldfld <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::battle
	IL_006b:  callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(<BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.>)
	IL_0070:  ldarg.0 
	IL_0071:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_0076:  callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
	IL_007b:  ret 
    } // end of method Music::PlayMusic

    // method line 17
    .method public hidebysig 
           instance default void StopMusic (float32 fadeSpeed)  cil managed 
    {
        // Method begins at RVA 0x2478
	// Code size 19 (0x13)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldarg.1 
	IL_0002:  stfld float32 Music::fadeSpeed
	IL_0007:  ldarg.0 
	IL_0008:  ldc.r4 0.
	IL_000d:  stfld float32 Music::desiredVolume
	IL_0012:  ret 
    } // end of method Music::StopMusic

    // method line 18
    .method private hidebysig 
           instance default void Update ()  cil managed 
    {
        // Method begins at RVA 0x248c
	// Code size 46 (0x2e)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_0006:  ldarg.0 
	IL_0007:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_000c:  callvirt instance float32 [UnityEngine.AudioModule]UnityEngine.AudioSource::get_volume()
	IL_0011:  ldarg.0 
	IL_0012:  ldfld float32 Music::desiredVolume
	IL_0017:  call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
	IL_001c:  ldarg.0 
	IL_001d:  ldfld float32 Music::fadeSpeed
	IL_0022:  mul 
	IL_0023:  call float32 [UnityEngine.CoreModule]UnityEngine.Mathf::Lerp(float32, float32, float32)
	IL_0028:  callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_volume(float32)
	IL_002d:  ret 
    } // end of method Music::Update

    // method line 19
    .method private hidebysig 
           instance default class [mscorlib]System.Collections.IEnumerator loadMusic ()  cil managed 
    {
        .custom instance void class [mscorlib]System.Runtime.CompilerServices.IteratorStateMachineAttribute::'.ctor'(class [mscorlib]System.Type) =  (
		01 00 16 4D 75 73 69 63 2B 3C 6C 6F 61 64 4D 75   // ...Music+<loadMu
		73 69 63 3E 64 5F 5F 31 34 00 00                ) // sic>d__14..

        // Method begins at RVA 0x24bb
	// Code size 14 (0xe)
	.maxstack 8
	IL_0000:  ldc.i4.0 
	IL_0001:  newobj instance void class Music/'<loadMusic>d__14'::'.ctor'(int32)
	IL_0006:  dup 
	IL_0007:  ldarg.0 
	IL_0008:  stfld class Music Music/'<loadMusic>d__14'::'<>4__this'
	IL_000d:  ret 
    } // end of method Music::loadMusic

    // method line 20
    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' ()  cil managed 
    {
        // Method begins at RVA 0x24ca
	// Code size 29 (0x1d)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldc.r4 1.
	IL_0006:  stfld float32 Music::fadeSpeed
	IL_000b:  ldarg.0 
	IL_000c:  ldc.r4 0.69999998807907104
	IL_0011:  stfld float32 Music::desiredVolume
	IL_0016:  ldarg.0 
	IL_0017:  call instance void [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::.ctor()
	IL_001c:  ret 
    } // end of method Music::.ctor

	.property class Music Instance ()
	{
		.get default class Music Music::get_Instance () 
		.set default void Music::set_Instance (class Music 'value') 
	}
  .class nested public auto ansi sealed MusicType
  	extends [netstandard]System.Enum
  {
    .field  public specialname rtspecialname  int32 value__
    .field public static literal  valuetype Music/MusicType Ambient = int32(0x00000000)
    .field public static literal  valuetype Music/MusicType Intense = int32(0x00000001)
    .field public static literal  valuetype Music/MusicType Battle = int32(0x00000002)

  } // end of class MusicType

  .class nested private auto ansi sealed beforefieldinit '<loadMusic>d__14'
  	extends [netstandard]System.Object
  	implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>, [netstandard]System.Collections.IEnumerator, [netstandard]System.IDisposable  {
    .custom instance void class [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::'.ctor'() =  (01 00 00 00 ) // ....

    .field  private  int32 '<>1__state'
    .field  private  object '<>2__current'
    .field  public  class Music '<>4__this'
    .field  private  <BROKEN CLASS token_ 1000017 due to Could not load file or assembly 'UnityEngine.UnityWebRequestWWWModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> '<www>5__2'
    .field  private  <BROKEN CLASS token_ 1000017 due to Could not load file or assembly 'UnityEngine.UnityWebRequestWWWModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> '<www2>5__3'
    .field  private  <BROKEN CLASS token_ 1000017 due to Could not load file or assembly 'UnityEngine.UnityWebRequestWWWModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> '<www3>5__4'

    // method line 547
    .method public hidebysig specialname rtspecialname 
           instance default void '.ctor' (int32 '<>1__state')  cil managed 
    {
        .custom instance void class [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::'.ctor'() =  (01 00 00 00 ) // ....

        // Method begins at RVA 0xb528
	// Code size 14 (0xe)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call instance void object::'.ctor'()
	IL_0006:  ldarg.0 
	IL_0007:  ldarg.1 
	IL_0008:  stfld int32 Music/'<loadMusic>d__14'::'<>1__state'
	IL_000d:  ret 
    } // end of method <loadMusic>d__14::.ctor

    // method line 548
    .method private final virtual hidebysig newslot 
           instance default void System.IDisposable.Dispose ()  cil managed 
    {
        .custom instance void class [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::'.ctor'() =  (01 00 00 00 ) // ....

        // Method begins at RVA 0x20f3
	.override class [mscorlib]System.IDisposable::Dispose
	// Code size 1 (0x1)
	.maxstack 8
	IL_0000:  ret 
    } // end of method <loadMusic>d__14::System.IDisposable.Dispose

    // method line 549
    .method private final virtual hidebysig newslot 
           instance default bool MoveNext ()  cil managed 
    {
        // Method begins at RVA 0xb538
	.override class [mscorlib]System.Collections.IEnumerator::MoveNext
	// Code size 239 (0xef)
	.maxstack 4
	.locals init (
		int32	V_0,
		class Music	V_1,
		string	V_2)
	IL_0000:  ldarg.0 
	IL_0001:  ldfld int32 Music/'<loadMusic>d__14'::'<>1__state'
	IL_0006:  stloc.0 
	IL_0007:  ldarg.0 
	IL_0008:  ldfld class Music Music/'<loadMusic>d__14'::'<>4__this'
	IL_000d:  stloc.1 
	IL_000e:  ldloc.0 
	IL_000f:  brfalse.s IL_0017

	IL_0011:  ldloc.0 
	IL_0012:  ldc.i4.1 
	IL_0013:  beq.s IL_0094

	IL_0015:  ldc.i4.0 
	IL_0016:  ret 
	IL_0017:  ldarg.0 
	IL_0018:  ldc.i4.m1 
	IL_0019:  stfld int32 Music/'<loadMusic>d__14'::'<>1__state'
	IL_001e:  call string [UnityEngine.CoreModule]UnityEngine.Application::get_dataPath()
	IL_0023:  ldstr "/Resources/Music"
	IL_0028:  call string string::Concat(string, string)
	IL_002d:  stloc.2 
	IL_002e:  ldarg.0 
	IL_002f:  ldstr "file:/"
	IL_0034:  ldloc.2 
	IL_0035:  ldstr "/ambient.ogg"
	IL_003a:  call string string::Concat(string, string, string)
	IL_003f:  newobj instance void [UnityEngine.UnityWebRequestWWWModule]UnityEngine.WWW::.ctor(string)
	IL_0044:  stfld <BROKEN CLASS token_ 1000017 due to Could not load file or assembly 'UnityEngine.UnityWebRequestWWWModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music/'<loadMusic>d__14'::'<www>5__2'
	IL_0049:  ldarg.0 
	IL_004a:  ldstr "file:/"
	IL_004f:  ldloc.2 
	IL_0050:  ldstr "/intense.ogg"
	IL_0055:  call string string::Concat(string, string, string)
	IL_005a:  newobj instance void [UnityEngine.UnityWebRequestWWWModule]UnityEngine.WWW::.ctor(string)
	IL_005f:  stfld <BROKEN CLASS token_ 1000017 due to Could not load file or assembly 'UnityEngine.UnityWebRequestWWWModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music/'<loadMusic>d__14'::'<www2>5__3'
	IL_0064:  ldarg.0 
	IL_0065:  ldstr "file:/"
	IL_006a:  ldloc.2 
	IL_006b:  ldstr "/battle.ogg"
	IL_0070:  call string string::Concat(string, string, string)
	IL_0075:  newobj instance void [UnityEngine.UnityWebRequestWWWModule]UnityEngine.WWW::.ctor(string)
	IL_007a:  stfld <BROKEN CLASS token_ 1000017 due to Could not load file or assembly 'UnityEngine.UnityWebRequestWWWModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music/'<loadMusic>d__14'::'<www3>5__4'
	IL_007f:  ldarg.0 
	IL_0080:  ldarg.0 
	IL_0081:  ldfld <BROKEN CLASS token_ 1000017 due to Could not load file or assembly 'UnityEngine.UnityWebRequestWWWModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music/'<loadMusic>d__14'::'<www>5__2'
	IL_0086:  stfld object Music/'<loadMusic>d__14'::'<>2__current'
	IL_008b:  ldarg.0 
	IL_008c:  ldc.i4.1 
	IL_008d:  stfld int32 Music/'<loadMusic>d__14'::'<>1__state'
	IL_0092:  ldc.i4.1 
	IL_0093:  ret 
	IL_0094:  ldarg.0 
	IL_0095:  ldc.i4.m1 
	IL_0096:  stfld int32 Music/'<loadMusic>d__14'::'<>1__state'
	IL_009b:  ldloc.1 
	IL_009c:  ldarg.0 
	IL_009d:  ldfld <BROKEN CLASS token_ 1000017 due to Could not load file or assembly 'UnityEngine.UnityWebRequestWWWModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music/'<loadMusic>d__14'::'<www>5__2'
	IL_00a2:  ldc.i4.0 
	IL_00a3:  callvirt instance <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> [UnityEngine.UnityWebRequestWWWModule]UnityEngine.WWW::GetAudioClip(bool)
	IL_00a8:  stfld <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::ambient
	IL_00ad:  ldloc.1 
	IL_00ae:  ldarg.0 
	IL_00af:  ldfld <BROKEN CLASS token_ 1000017 due to Could not load file or assembly 'UnityEngine.UnityWebRequestWWWModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music/'<loadMusic>d__14'::'<www2>5__3'
	IL_00b4:  ldc.i4.0 
	IL_00b5:  callvirt instance <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> [UnityEngine.UnityWebRequestWWWModule]UnityEngine.WWW::GetAudioClip(bool)
	IL_00ba:  stfld <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::intense
	IL_00bf:  ldloc.1 
	IL_00c0:  ldarg.0 
	IL_00c1:  ldfld <BROKEN CLASS token_ 1000017 due to Could not load file or assembly 'UnityEngine.UnityWebRequestWWWModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music/'<loadMusic>d__14'::'<www3>5__4'
	IL_00c6:  ldc.i4.0 
	IL_00c7:  callvirt instance <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> [UnityEngine.UnityWebRequestWWWModule]UnityEngine.WWW::GetAudioClip(bool)
	IL_00cc:  stfld <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::battle
	IL_00d1:  ldloc.1 
	IL_00d2:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_00d7:  ldloc.1 
	IL_00d8:  ldfld <BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::ambient
	IL_00dd:  callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(<BROKEN CLASS token_ 100000e due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.>)
	IL_00e2:  ldloc.1 
	IL_00e3:  ldfld <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> Music::music
	IL_00e8:  callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
	IL_00ed:  ldc.i4.0 
	IL_00ee:  ret 
    } // end of method <loadMusic>d__14::MoveNext

    // method line 550
    .method private final virtual hidebysig newslot specialname 
           instance default object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' ()  cil managed 
    {
        .custom instance void class [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::'.ctor'() =  (01 00 00 00 ) // ....

        // Method begins at RVA 0xb633
	.override method instance !0 class [mscorlib]System.Collections.Generic.IEnumerator`1<object>::get_Current()
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldfld object Music/'<loadMusic>d__14'::'<>2__current'
	IL_0006:  ret 
    } // end of method <loadMusic>d__14::System.Collections.Generic.IEnumerator<System.Object>.get_Current

    // method line 551
    .method private final virtual hidebysig newslot 
           instance default void System.Collections.IEnumerator.Reset ()  cil managed 
    {
        .custom instance void class [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::'.ctor'() =  (01 00 00 00 ) // ....

        // Method begins at RVA 0xb63b
	.override class [mscorlib]System.Collections.IEnumerator::Reset
	// Code size 6 (0x6)
	.maxstack 8
	IL_0000:  newobj instance void class [mscorlib]System.NotSupportedException::'.ctor'()
	IL_0005:  throw 
    } // end of method <loadMusic>d__14::System.Collections.IEnumerator.Reset

    // method line 552
    .method private final virtual hidebysig newslot specialname 
           instance default object System.Collections.IEnumerator.get_Current ()  cil managed 
    {
        .custom instance void class [mscorlib]System.Diagnostics.DebuggerHiddenAttribute::'.ctor'() =  (01 00 00 00 ) // ....

        // Method begins at RVA 0xb633
	.override class [mscorlib]System.Collections.IEnumerator::get_Current
	// Code size 7 (0x7)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  ldfld object Music/'<loadMusic>d__14'::'<>2__current'
	IL_0006:  ret 
    } // end of method <loadMusic>d__14::System.Collections.IEnumerator.get_Current

	.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current' ()
	{
		.get instance default object Music/'<loadMusic>d__14'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () 
	}
	.property instance object System.Collections.IEnumerator.Current ()
	{
		.get instance default object Music/'<loadMusic>d__14'::System.Collections.IEnumerator.get_Current () 
	}
  } // end of class <loadMusic>d__14

  } // end of class Music

  .class public auto ansi beforefieldinit PlayerAudio
  	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
  {
    .field  private  <BROKEN CLASS token_ 1000009 due to Could not load file or assembly 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> rb
    .field  public  <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> wind
    .field  public  <BROKEN CLASS token_ 1000006 due to Could not load file or assembly 'UnityEngine.AudioModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.> foley
    .field  private  float32 currentVol
    .field  private  float32 volVel

    // method line 21
    .method private hidebysig 
           instance default void Start ()  cil managed 
    {
        // Method begins at RVA 0x24e8
	// Code size 17 (0x11)
	.maxstack 8
	IL_0000:  ldarg.0 
	IL_0001:  call class PlayerMovement class PlayerMovement::get_Instance()
	IL_0006:  callvirt Could not load signature of PlayerMovement:GetRb due to: Could not load file or assembly 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' or one of its dependencies.
