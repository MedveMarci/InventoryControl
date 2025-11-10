# InventoryControl
[![Version](https://img.shields.io/github/v/release/MedveMarci/InventoryControl?sort=semver&label=Version)](https://github.com/MedveMarci/InventoryControl/releases)
[![Downloads](https://img.shields.io/github/downloads/MedveMarci/InventoryControl/total?label=Downloads)](https://github.com/MedveMarci/InventoryControl/releases)
<a href="https://github.com/KenleyundLeon/DeltaPatch"><img src="https://image2url.com/images/1759565889245-ff2e02c2-1f19-4f72-bc06-43a3b77fb4bd.png"></a>

An SCP:Secret Laboratory plugin which allows you to configure the default role inventory

## How download ?
   - *1. Find the SCP SL server config folder*
   
   *("C:\Users\(user name)\AppData\Roaming\SCP Secret Laboratory\" for windows, "/home/(user name)/.config/SCP Secret Laboratory/" for linux)*
  
   - *2. Find the "LabAPI" folder there, it contains the "plugins" folder.*
  
   - *3. Select either the port of your server to install the same on that server or the "global" folder to install the plugin for all servers*

## Config

```yml
# Custom inventory list for the role. (Do not add a role to the list if you want to leave the role as a regular inventory)
inventory:
  DefaultCassD:
    role_type_id: ClassD
    keep_items: false
    keep_ammos: false
    items:
      Painkillers: 80
      Coin: 100
    ammos: 
  JanitorCassD:
    role_type_id: ClassD
    keep_items: false
    keep_ammos: false
    items:
      KeycardJanitor: 35
      Painkillers: 80
      Coin: 100
    ammos: 
  DefaultScientist:
    role_type_id: Scientist
    keep_items: true
    keep_ammos: false
    items:
      Flashlight: 100
      Coin: 90
    ammos: 
# Custom inventory list for players with a rank
inventory_rank:
  owner:
    OwnerCassD:
      role_type_id: ClassD
      keep_items: false
      keep_ammos: false
      items:
        KeycardScientist: 80
        GunCOM18: 40
        Painkillers: 100
        Coin: 100
      ammos:
        Ammo9x19: 30
    OwnerScientist:
      role_type_id: Scientist
      keep_items: true
      keep_ammos: false
      items:
        GunCOM18: 65
        SCP500: 70
        Flashlight: 100
        Coin: 90
      ammos:
        Ammo9x19: 60
```


```
InventoryName:
   role_type_id: RoleType
   keep_items: true / false
   keep_ammos: true / false
   items:
      ItemType: Chance
   ammos:
      AmmoType: Amount
```

```
RankName:
   InventoryName:
      role_type_id: RoleType
      keep_items: true / false
      keep_ammos: true / false
      items:
         ItemType: Chance
      ammos:
         AmmoType: Amount
```

## Types

**RoleType**
```
Scp173,
ClassD,
Spectator,
Scp106,
NtfSpecialist,
Scp049,
Scientist,
Scp079,
ChaosConscript,
Scp096,
Scp0492,
NtfSergeant,
NtfCaptain,
NtfPrivate,
Tutorial,
FacilityGuard,
Scp939,
ChaosRifleman,
ChaosRepressor,
ChaosMarauder
```

**ItemType**
```
KeycardJanitor
KeycardScientist
KeycardResearchCoordinator
KeycardZoneManager
KeycardGuard
KeycardMTFPrivate
KeycardContainmentEngineer
KeycardMTFOperative
KeycardMTFCaptain
KeycardFacilityManager
KeycardChaosInsurgency
KeycardO5
Radio
GunCOM15
Medkit
Flashlight
MicroHID
SCP500
SCP207
Ammo12gauge
GunE11SR
GunCrossvec
Ammo556x45
GunFSP9
GunLogicer
GrenadeHE
GrenadeFlash
Ammo44cal
Ammo762x39
Ammo9x19
GunCOM18
SCP018
SCP268
Adrenaline
Painkillers
Coin
ArmorLight
ArmorCombat
ArmorHeavy
GunRevolver
GunAK
GunShotgun
SCP330
SCP2176
SCP244a
SCP244b
SCP1853
ParticleDisruptor
GunCom45
SCP1576
Jailbird
AntiSCP207
GunFRMG0
GunA7
Lantern
SCP1344
Snowball
Coal
SpecialCoal
SCP1507Tape
DebugRagdollMover
SurfaceAccessPass
GunSCP127
KeycardCustomTaskForce
KeycardCustomSite02
KeycardCustomManagement
KeycardCustomMetalCase
MarshmallowItem
SCP1509
```
