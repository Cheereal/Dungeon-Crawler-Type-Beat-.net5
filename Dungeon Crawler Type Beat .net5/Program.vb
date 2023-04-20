Imports System
Imports System.Threading
Imports System.Text.Json
Imports System.IO
Imports System.Text.Json.Serialization
Imports Newtonsoft.Json

'To Do:
'item, enemy and weapon descriptions (json or txt file) -Done (finally)
'rest of story -Doing along with other stuff
'weapon balancing (100% gonna need to do this lmao)
'add enemies - Done
Module Program
    Dim Playing As Boolean = True
    Dim floor As Integer = 1
    Dim go1 As Boolean = False
    Dim go2 As Boolean = False
    Dim pname As String
    Dim phealth As Integer = 100
    Dim maxphealth As Integer = 100
    Dim maxpmana As Integer = 20
    Dim pmana As Integer = 20
    Dim dambonus As Integer = 0
    Dim defbonus As Integer = 0
    Dim pattackbonus As Integer = 0
    Dim pdefencebonus As Integer = 0
    Dim phealthbonus As Integer = 0
    Dim pmanabonus As Integer = 0
    Dim inv As New List(Of String)({})
    Dim iteminv As New List(Of String)({"small health potion", "small health potion"})
    Dim maginv As New List(Of String)({})
    Dim ehp As Integer = 0
    Dim healmag As New List(Of String)({"basic heal", "advanced heal", "master heal"})
    Dim weaponstats = {{"copper short sword", 7, 18, 6}, {"copper long sword", 5, 20, 7}, {"kunai", 6, 18, 4}, {"short bow", 3, 19, 5}, {"crossbow", 7, 25, 9}, {"mace", 5, 15, 4}, {"spear", 5, 16, 6}, {"boomerang", 10, 25, 9}, {"knife", 6, 18, 7}, {"claymore", 5, 30, 8}, {"dagger", 4, 12, 4}, {"throwing knife", 8, 22, 8}, {"iron long sword", 8, 24, 7}, {"iron short sword", 9, 20, 6}, {"javelin", 10, 25, 8}, {"long bow", 6, 21, 6}, {"stick", 4, 5, 1}, {"shield", 6, 18, 3}, {"mythril long sword", 17, 30, 7}, {"mythril short sword", 13, 26, 6}, {"scythe", 8, 24, 8}, {"sickle", 10, 20, 6}, {"steel long sword", 14, 27, 7}, {"steel short sword", 13, 24, 6}, {"magic katana", 16, 28, 5}, {"magic kunai", 14, 26, 5}, {"magic long bow", 10, 25, 5}, {"magic long sword", 16, 28, 6}, {"magic short bow", 6, 22, 4}, {"magic short sword", 11, 24, 5}, {"molotov", 20, 35, 9}, {"enchanted shield", 8, 28, 5}, {"biiiiig sword", 25, 37, 6}, {"blood butcherer", 22, 39, 7}, {"chain sword", 30, 40, 9}, {"deagle", 30, 37, 8}, {"reflective shield", 13, 30, 4}, {"l115a3", 42, 55, 5}, {"gun shaped stick", 45, 57, 6}, {"holy hand grenade", 45, 60, 8}, {"slightly bigger stick", 45, 50, 2}, {"midas sword", 30, 70, 5}} 'weapon name, min damage, max damage, hit/20 (if above hit is a hit)
    Dim magicstats = {{"basic heal", 5, 17, 0, 6}, {"conjure rock", 14, 26, 7, 7}, {"electric shock", 10, 20, 8, 8}, {"fireball", 13, 26, 8, 6}, {"generate wind", 5, 10, 6, 5}, {"water ball", 12, 24, 7, 6}, {"advanced heal", 15, 30, 0, 8}, {"conjure big rock", 20, 40, 8, 12}, {"electric ball", 20, 34, 6, 10}, {"firewall", 23, 32, 7, 9}, {"twister", 18, 25, 5, 8}, {"waterbolt", 16, 36, 8, 9}, {"master heal", 25, 45, 0, 14}, {"earthquake", 35, 55, 8, 17}, {"electric wave", 30, 48, 7, 16}, {"inferno", 40, 64, 10, 19}, {"tornado", 25, 54, 7, 16}, {"tsunami", 42, 53, 6, 19}} 'magic name, min damage/heal, max damage/heal, hit/20 (if above hit is a hit), mana used
    Dim itemstats = {{"small health potion", 25, "heal"}, {"small damage potion", 3, "damage"}, {"small defence potion", 5, "defence"}, {"small mana potion", 5, "mana"}, {"medium health potion", 50, "heal"}, {"medium damage potion", 8, "damage"}, {"medium defence potion", 13, "defence"}, {"medium mana potion", 15, "mana"}, {"large health potion", 999, "heal"}, {"large damage potion", 15, "damage"}, {"large defence potion", 18, "defence"}, {"large mana potion", 999, "mana"}} 'item name, hp/damageboost/damageblock ,item type
    Dim enemyentry As New List(Of String)({})
    Dim pre As String = "The"
    Sub delayprint(x As String, y As Integer, sleep As Integer)
        For Each letter As String In x
            Console.Write(letter)
            Thread.Sleep(y)
        Next
        Console.WriteLine("")
        Thread.Sleep(sleep)
    End Sub
    Sub description(choice As String)
        Dim jsondesc As String
        Using streamreader As New StreamReader("desc.json")
            jsondesc = streamreader.ReadToEnd()
        End Using
        Dim weaponDescs As New Dictionary(Of String, String)(Text.Json.JsonSerializer.Deserialize(Of Dictionary(Of String, String))(jsondesc))
        Try
            delayprint($"{weaponDescs(choice)}", 3, 500)
        Catch
            delayprint("You do not have an entry for this item!", 30, 500)
        End Try
    End Sub
    Sub Loot(floor As Integer, overrideloot As Integer, getweapoverride As Integer)
        Dim r As Random = New Random()
        delayprint("You found some items inside!", 30, 500)
        Dim gotitem As Integer = r.Next(1, 5)
        Dim rarity As String
        Select Case floor
            Case Is = 1, 2
                rarity = "small"
            Case Is = 3, 4
                rarity = "medium"
            Case Is = 5
                rarity = "large"
            Case Else
                rarity = "error wtf happened"
        End Select
        Select Case gotitem
            Case Is = 1 'health
                iteminv.Add(rarity + " health potion")
                delayprint($"Voice: 'That's a {rarity} health potion'", 30, 500)
            Case Is = 2 'damage
                iteminv.Add(rarity + " damage potion")
                delayprint($"Voice: 'That's a {rarity} damage potion'", 30, 500)
            Case Is = 3 'defence
                iteminv.Add(rarity + " defence potion")
                delayprint($"Voice: 'That's a {rarity} defence potion'", 30, 500)
            Case Is = 4 'mana
                iteminv.Add(rarity + " mana potion")
                delayprint($"Voice: 'That's a {rarity} mana potion'", 30, 500)
        End Select
        delayprint($"You now have {Listprint(iteminv)} In your inventory!", 30, 75)
        Dim gotweap As Integer = r.Next(1, 3)
        If getweapoverride = 1 Then
            gotweap = 1
        End If
        If gotweap = 1 Then
            delayprint("You found a weapon inside!", 30, 500)
            Dim rarp As Integer
            Dim epip As Integer
            Dim MULp As Integer
            Select Case floor
                Case Is = 1, 2, 3
                    rarp = 35
                    epip = 0
                    MULp = 0
                Case Is = 3
                    rarp = 50
                    epip = 8
                    MULp = 2
                Case Is = 4
                    rarp = 56
                    epip = 16
                    MULp = 4
                Case Is = 5
                    rarp = 10
                    epip = 65
                    MULp = 20
            End Select
            Dim chance As Integer = r.Next(0, 100)
            Dim weaponrarity As Integer
            If chance <= MULp Then
                weaponrarity = 3 'got mega ultra legendary epic 360 fortnite gamer boss weapon
            ElseIf chance > MULp And chance < (epip + MULp) Then
                weaponrarity = 2 'got epic weapon
            ElseIf chance > (rarp + MULp) And chance < (rarp + epip + MULp) Then
                weaponrarity = 1 'got rare weapon
            ElseIf chance > (rarp + epip + MULp) Then
                weaponrarity = 0 'got common weapon
            Else
                weaponrarity = 0
            End If
            Select Case overrideloot
                Case Is = 1
                    weaponrarity = 1 'rare weapon
                Case Is = 2
                    weaponrarity = 2 'epic weapon
                Case Is = 3
                    weaponrarity = 3 'MULLEFGB weapon
            End Select
            Dim weapgot As Integer
            Dim weap As String = ""
            Select Case weaponrarity
                Case Is = 0
                    delayprint("Voice: 'Thats a common rarity weapon'", 30, 500)
                    weapgot = r.Next(1, 19)
                    Dim weapontype As Integer = r.Next(1, 101)
                    Dim weaponmat As Integer
                    Select Case weapontype
                        Case Is <= 55
                            weaponmat = 0 'copper
                        Case Is > 55 And (weapontype < 80)
                            weaponmat = 1 'iron
                        Case Is > 80 And (weapontype < 95)
                            weaponmat = 2 'steel
                        Case Is >= 95
                            weaponmat = 3 'mythril
                    End Select
                    Select Case weapgot
                        Case Is = 1 'short swords
                            Select Case weaponmat
                                Case Is = 0
                                    weap = "copper short sword"
                                Case Is = 1
                                    weap = "iron short sword"
                                Case Is = 2
                                    weap = "steel short sword"
                                Case Is = 3
                                    weap = "mythril short sword"
                            End Select
                        Case Is = 2 'long swords
                            Select Case weaponmat
                                Case Is = 0
                                    weap = "copper long sword"
                                Case Is = 1
                                    weap = "iron long sword"
                                Case Is = 2
                                    weap = "steel long sword"
                                Case Is = 3
                                    weap = "mythril long sword"
                            End Select
                        Case Is = 3 'short bow
                            weap = "short bow"
                        Case Is = 4 'long bow
                            weap = "long bow"
                        Case Is = 5 'daggers
                            weap = "dagger"
                        Case Is = 6 'throwing knife
                            weap = "throwing knife"
                        Case Is = 7 'claymore
                            weap = "claymore"
                        Case Is = 8 'boomerang
                            weap = "boomerang"
                        Case Is = 9 'mace
                            weap = "mace"
                        Case Is = 10 'spear
                            weap = "spear"
                        Case Is = 11 'crossbow
                            weap = "crossbow"
                        Case Is = 12 'stick
                            weap = "stick"
                        Case Is = 13 'javelin
                            weap = "javelin"
                        Case Is = 14 'knife
                            weap = "knife"
                        Case Is = 15 'sickle
                            weap = "sickle"
                        Case Is = 16 'scythe
                            weap = "scythe"
                        Case Is = 17 'kunai
                            weap = "kunai"
                        Case Is = 18 'shield
                            weap = "shield"
                    End Select
                Case Is = 1
                    delayprint("Voice: 'Thats a rare rarity weapon'", 30, 500)
                    weapgot = r.Next(1, 9)
                    Select Case weapgot
                        Case Is = 1 'Magic katana
                            weap = "magic katana"
                        Case Is = 2 'Magic short sword
                            weap = "magic short sword"
                        Case Is = 3 'Magic long sword
                            weap = "magic long sword"
                        Case Is = 4 'magic short bow
                            weap = "magic short bow"
                        Case Is = 5 'magic long bow
                            weap = "magic long bow"
                        Case Is = 6 'magic kunai
                            weap = "magic kunai"
                        Case Is = 7 'Molotovs
                            weap = "molotov"
                        Case Is = 8 'Enchanted Shield
                            weap = "enchanted shield"
                    End Select
                Case Is = 2
                    delayprint("Voice: 'Thats an Epic rarity weapon'", 30, 500)
                    weapgot = r.Next(1, 6)
                    Select Case weapgot
                        Case Is = 1 'Deagle
                            weap = "deagle"
                        Case Is = 2 'BIIIIIG sword
                            weap = "biiiiig sword"
                        Case Is = 3 'Reflective shield
                            weap = "reflective shield"
                        Case Is = 4 'Blood Butcherer
                            weap = "blood butcherer"
                        Case Is = 5 'Chain Sword
                            weap = "chain sword"
                    End Select
                Case Is = 3
                    delayprint("Voice: 'Thats a Mega Ultra Legendary Epic 360 fortnite gamer boss rarity weapon'", 30, 500)
                    weapgot = r.Next(1, 6)
                    Select Case weapgot
                        Case Is = 1 'Holy Hand Grenade
                            weap = "holy hand grenade"
                        Case Is = 2 'Slightly bigger stick
                            weap = "slightly bigger stick"
                        Case Is = 3 'Gun Shaped Stick
                            weap = "gun shaped stick"
                        Case Is = 4 'L115A3
                            weap = "l115a3"
                        Case Is = 5 'Midas sword
                            weap = "midas sword"
                    End Select
            End Select
            If Not inv.Contains(weap.ToLower) Then
                inv.Add(weap.ToLower)
            End If
            If gotweap = 1 Then
                delayprint($"You found a {weap}", 30, 500)
                delayprint($"You have these weapons now: {Listprint(inv)}", 10, 75)
            End If
        End If
    End Sub

    Function Listprint(list As List(Of String)) As String
        Dim items As String = ""
        For x As Integer = 0 To list.Count - 1
            If x = list.Count - 1 Then
                items += list(x) + "."
            Else
                items += list(x) + ", "
            End If
        Next
        Return items
    End Function

    Sub Fight(ename As String, weaponmax As Integer, weaponmin As Integer, weaponhit As Integer, enemyhp As Integer, enemymin As Integer, enemymax As Integer)
        Dim r As Random = New Random()
        If r.Next(1, 20) >= weaponhit Then
            Dim dmg As Integer = r.Next(weaponmin, weaponmax + 1) + dambonus
            ehp -= dmg
            delayprint($"You hit {pre} {ename} for {dmg}HP!", 30, 250)
        Else
            delayprint("You missed your attack!", 30, 250)
        End If
        Console.WriteLine()
    End Sub

    Sub Heal(weaponmax As Integer, weaponmin As Integer, php As Integer)
        Dim r As Random = New Random()
        Dim pheal As Integer = r.Next(weaponmin, weaponmax + 1)
        phealth += pheal
        If phealth > maxphealth Then
            phealth = maxphealth
        End If
        delayprint($"You used a magic spell and healed for {pheal}HP! You are now on {phealth}HP!", 30, 500)
    End Sub

    Function useitem(item As String, itemstat As Integer, itemtype As String)
        Select Case itemtype
            Case Is = "heal"
                If phealth <> maxphealth Then
                    delayprint($"You used a {item} and restored {itemstat}HP!", 30, 500)
                    phealth += itemstat
                    iteminv.Remove(item)
                    If phealth > maxphealth Then
                        phealth = maxphealth
                    End If
                    delayprint($"You are now on {phealth}HP!", 30, 500)
                    Return 1
                Else
                    delayprint("You are on max health", 30, 500)
                    Return 0
                End If
            Case Is = "damage"
                delayprint($"You used a {item} and now have a +{itemstat} damage bonus!", 30, 500)
                iteminv.Remove(item)
                dambonus = itemstat
                Return 1
            Case Is = "defence"
                delayprint($"You used a {item} and now have a +{itemstat} defence bonus!", 30, 500)
                iteminv.Remove(item)
                defbonus = itemstat
                Return 1
            Case Is = "mana"
                If pmana <> maxpmana Then
                    delayprint($"You used a {item} and restored {itemstat} mana!", 30, 500)
                    pmana += itemstat
                    iteminv.Remove(item)
                    If pmana > maxpmana Then
                        pmana = maxpmana
                    End If
                    delayprint($"You are now on {pmana} mana!", 30, 500)
                    Return 1
                Else
                    delayprint("You are on max mana", 30, 500)
                    Return 0
                End If
            Case Else
                Return 0
        End Select
    End Function

    Sub battle(ename As String, Optional remthe As Boolean = False)
        If remthe = True Then
            pre = ""
        End If
        go1 = True
        Dim enstats = Grabstats(ename)
        ehp = enstats.health
        If Not enemyentry.Contains(ename.ToLower) Then
            enemyentry.Add(ename.ToLower)
        End If
        While go1 = True
            go2 = True
            While go2 = True
                Dim choices As New List(Of String)({"Attack", "Magic", "Check Inventory", "Dictionary"})
                delayprint($"What would you like to do? {Listprint(choices)} HP:{phealth}", 30, 75)
                Dim choice As String = Console.ReadLine.ToLower()
                If choice = "attack" Then
                    delayprint("What weapon would you like to use?", 30, 300)
                    delayprint($"You currently have these weapons: {Listprint(inv)}", 10, 75)
                    Dim weapon As String = Console.ReadLine().ToLower
                    If Not inv.Contains(weapon) Then
                        delayprint("You do not have this weapon!", 30, 500)
                    End If
                    For index0 = 0 To weaponstats.GetUpperBound(0)
                        If weaponstats(index0, 0) = weapon And inv.Contains(weapon) Then
                            Dim weaponmin = weaponstats(index0, 1)
                            Dim weaponmax = weaponstats(index0, 2)
                            Dim weaponhit = weaponstats(index0, 3)
                            Fight(enstats.name, weaponmax, weaponmin, weaponhit, ehp, enstats.damagerange(0), enstats.damagerange(1))
                            go2 = False
                            If ehp <= 0 Then
                                go1 = False
                                delayprint($"You killed {pre} {enstats.name}", 30, 500)
                                go2 = False
                            End If
                        End If
                    Next
                ElseIf choice = "magic" Then
                    delayprint("What magic would you like to use?", 30, 300)
                    Try
                        delayprint($"You currently have these spells: {Listprint(maginv)} and are on {pmana} mana!", 10, 75)
                    Catch
                        delayprint("You don't have any magic!", 30, 300)
                    End Try
                    Dim weapon As String = Console.ReadLine()
                    If Not maginv.Contains(weapon) Then
                        delayprint("You do not have this magic!", 30, 300)
                    End If
                    For index0 = 0 To magicstats.GetUpperBound(0)
                        If magicstats(index0, 0) = weapon Then
                            Dim weaponmin = magicstats(index0, 1)
                            Dim weaponmax = magicstats(index0, 2)
                            Dim weaponhit = magicstats(index0, 3)
                            If pmana - magicstats(index0, 4) >= 0 And maginv.Contains(weapon) Then
                                If healmag.Contains(weapon) Then
                                    pmana -= magicstats(index0, 4)
                                    Heal(weaponmax, weaponmin, phealth)
                                    go2 = False
                                Else
                                    pmana -= magicstats(index0, 4)
                                    Fight(enstats.name, weaponmax, weaponmin, weaponhit, ehp, enstats.damagerange(0), enstats.damagerange(1))
                                    delayprint($"You are now on {pmana} mana!", 30, 300)
                                    go2 = False
                                End If
                            End If
                            If ehp <= 0 Then
                                go1 = False
                                delayprint($"You killed {pre} {enstats.name}", 30, 300)
                                pmanabonus = 0
                                pattackbonus = 0
                                pdefencebonus = 0
                                phealthbonus = 0
                            End If

                        End If
                    Next
                ElseIf choice = "check inventory" Then
                    delayprint($"You currently have: {Listprint(iteminv)}", 10, 75)
                    delayprint("What item would you like to use?", 30, 300)
                    Dim itemuse As String = Console.ReadLine()
                    For index0 = 0 To itemstats.GetUpperBound(0)
                        If itemstats(index0, 0) = itemuse And iteminv.Contains(itemuse) Then
                            Dim itemstat = itemstats(index0, 1)
                            Dim itemtype = itemstats(index0, 2)
                            Dim endt As Boolean = useitem(itemuse, itemstat, itemtype)
                            If endt = 1 Then
                                go2 = False
                            End If
                        End If
                    Next
                ElseIf choice = "dictionary" Then
                    delayprint($"You currently have the weapon entries for {Listprint(inv)} {Listprint(maginv)}", 10, 500)
                    delayprint($"You also have enemy entries for {Listprint(enemyentry)}", 10, 500)
                    delayprint("Which entry would you like to read?", 30, 500)
                    Dim entry As String = Console.ReadLine()
                    If inv.Contains(entry) Or enemyentry.Contains(entry) Or maginv.Contains(entry) Then
                        description(entry)
                    Else
                        delayprint("You do not have an entry for this item", 30, 500)
                    End If
                    Console.WriteLine("")
                End If
            End While
            If go1 <> False Then
                go2 = True
            End If
            While go2 = True
                Dim r As Random = New Random()
                If r.Next(1, 21) >= enstats.hit Then
                    Dim edmg As Integer = r.Next(enstats.damagerange(0), enstats.damagerange(1) + 1) - defbonus
                    phealth -= edmg
                    delayprint($"{pre} {enstats.name} hit you for {edmg}HP! You are now on {phealth}HP!", 30, 250)
                Else
                    delayprint($"{pre} {enstats.name} tried to attack you but missed!", 30, 250)
                End If
                If phealth <= 0 Then
                    go1 = False
                    Playing = False
                    delayprint($"You died to {pre} {enstats.name}!", 30, 500)
                    Console.Clear()
                    delayprint("Game over lol", 30, 500)
                    End
                End If
                go2 = False
            End While
        End While
    End Sub

    Sub Scenario(scene, int, str)
        Select Case scene
            Case Is = 1
                'floor signs
                delayprint($"You enter through the large door, it reads {str}.", 30, 500)
            Case Is = 2
                'enemy appears
                delayprint($"A {str} has appeared before you!", 30, 500)
            Case Is = 3
                'healing crystal
                delayprint($"You found a healing crystal and used it to heal {int} HP!", 30, 500)
                phealth += int
                If phealth > maxphealth Then
                    phealth = maxphealth
                End If
                delayprint($"You are now on {phealth}HP!", 30, 500)
            Case Is = 4
                'boss start 1
                delayprint("Voice: 'I have a bad feeling with the next room...'", 30, 500)
                delayprint("Voice: 'This may be the boss room'", 30, 500)
                delayprint("...", 200, 500)
                delayprint($"You enter the door and find a{str}!", 30, 500)
            Case Is = 5
                'found chest
                delayprint("As you walk down the corridor you spot a chest!", 30, 500)
                delayprint("Voice: 'I wonder what could be inside...'", 30, 500)
                delayprint("You open the chest", 30, 500)
            Case Is = 6
                'end of boss heal + mana
                delayprint("You find a restoration crystal and use it to heal to max HP and max mana!", 30, 500)
                phealth = maxphealth
                pmana = maxpmana
                delayprint($"You are now on {phealth}HP and {pmana} mana!", 30, 500)
            Case Is = 7
                'go to next floor (dont use for last boss bc it would probably break bc loot thing)
                delayprint("Voice: 'I think that door leads to the next floor'", 30, 500)
                delayprint("You walk through the door and find a chest!", 30, 500)
                delayprint("You open the boss chest", 30, 500)
                If floor >= 3 Then
                    Loot(floor + 1, 2, 1)
                Else
                    Loot(floor + 1, floor, 1)
                End If
            Case Is = 8
                'mana crystal
                delayprint($"You found a mana crystal and used it to restore {int} mana!", 30, 500)
                pmana += int
                If pmana > maxpmana Then
                    pmana = maxpmana
                End If
                delayprint($"You are now on {pmana} mana!", 30, 500)
            Case Is = 9
                'final floor
                delayprint("Voice: 'This is the final floor, the enemies guarding the final room will be much stronger than before'", 40, 1000)
                delayprint("Voice: 'There seems to be a chest with a very rare weapon'", 30, 500)
                Loot(floor, 3, 1)
                'add new magic (max level) cant be bothered rn
                delayprint("Voice: 'I will also increase your maximum health and heal you to assist you in these upcoming battles.'", 30, 500)
                maxphealth += 20
                phealth = maxphealth
                delayprint($"You are now on {phealth}HP!", 30, 500)
                delayprint("Voice: 'I will increase your maximum mana too'", 30, 500)
                maxpmana += 15
                pmana = maxpmana
                delayprint($"You are now on {pmana} mana!", 30, 500)
                delayprint("Voice: 'Good luck'", 30, 500)
            Case Is = 10
                'final floor enemy appears
                delayprint($"Voice: 'Thats {str}! Be careful.'", 30, 500)
            Case Is = 11
                'final boss
                delayprint("Voice: 'Whatever has taken over this dungeon is in the room ahead. I can feel a large amount of power behind the door'", 30, 500)
                delayprint("Voice: 'It must be ඞ̵Φ̶̛͑͆͘α̵͑̎̑̂̂̌̒̀͂̎͛̽͛͐̏͐̽̒͐̋́͘̕̕͝͝Σ̵̨̡̢̨̩̻̼͚̻̮̘̳͎͚̘͖̰̺̰̠̹͛̍̈́̃̉̔̉̇̈́͑̓̉͒̒̉̂͋̔͐̽̈́̈́̒̾̑͊̆̅́̈̉͆̓̕̕̕̕͘͘͠͝ ", 30, 500)
                delayprint("Voice: 'It seems you cant hear its name yet, call it 'The Anomaly' For now'", 30, 500)
                delayprint("You walk over to a table and find some potions!", 30, 500)
                iteminv.Add("large health potion")
                iteminv.Add("large damage potion")
                iteminv.Add("large defence potion")
                iteminv.Add("large mana potion")
                delayprint($"You now have {Listprint(iteminv)} In your inventory!", 10, 75)
                delayprint("Voice: 'I am also able to give you a new, very powerful spell'", 30, 500)
                delayprint("Master Heal, Earthquake, Electric Wave, Inferno, Tornado, Tsunami", 30, 500)
                delayprint("Voice: 'So, what would you like?'", 30, 500)
                go1 = True
                While go1 = True
                    Dim choice = Console.ReadLine().ToLower
                    Select Case choice
                        Case Is = "master heal"
                            maginv.Add("master heal")
                            go1 = False
                        Case Is = "earthquake"
                            maginv.Add("earthquake")
                            go1 = False
                        Case Is = "electric wave"
                            maginv.Add("electric wave")
                            go1 = False
                        Case Is = "inferno"
                            maginv.Add("inferno")
                            go1 = False
                        Case Is = "tornado"
                            maginv.Add("tornado")
                            go1 = False
                        Case Is = "tsunami"
                            maginv.Add("tsunami")
                            go1 = False
                    End Select
                End While
                delayprint($"You now have: {Listprint(maginv)}", 30, 500)
                delayprint($"Voice: 'Good luck, {pname}'", 30, 1000)
                delayprint("You open the door...", 75, 1000)
                delayprint("The Anomaly Appears before you!", 30, 500)
            Case Is = 12
                'game end
                delayprint("Voice: 'You managed to do it!'", 30, 500)
                delayprint("Congratulations! You finished the dungeon and can now leave!", 30, 500)
                delayprint("Or can you...", 30, 2000)
                delayprint($"You ended with {phealth}HP with a maximum HP of {maxphealth} and {pmana} mana with a maximum mana of {maxpmana}", 30, 500)
                delayprint($"You had {Listprint(inv)}", 30, 3500)
                delayprint($"You also had {Listprint(maginv)}", 30, 2000)
                delayprint($"Finally in your item inventory you had {Listprint(iteminv)} left over!", 30, 500)
                delayprint("Finish", 30, 500)
        End Select
    End Sub

    Sub Intro()
        delayprint("You find yourself locked in an unfamiliar dungeon. You hear a voice telling you:", 40, 500)
        delayprint("Voice: 'The only way out is to escape through the tenth floor. Good luck'", 30, 1000)
        Console.WriteLine("")
        delayprint("An ancient slate appears before you. ", 30, 500)
        delayprint("What is your name? ", 30, 0)
        pname = Console.ReadLine().ToString
        delayprint($"The slate engraves '{pname}' into its stone ", 30, 1000)
        Console.WriteLine("")
        delayprint("Choose a starting weapon.", 30, 500)
        go1 = True
        While go1 = True
            delayprint("Your choices are: Copper Short Sword, Copper Long Sword, Short Bow. Choose wisely.", 30, 500)
            Dim choice As String = Console.ReadLine().ToLower
            If choice = "copper short sword" Then
                inv.Add("copper short sword")
                go1 = False
            ElseIf choice = "copper long sword" Then
                inv.Add("copper long sword")
                go1 = False
            ElseIf choice = "short bow" Then
                inv.Add("short bow")
                go1 = False
            End If
        End While
        delayprint($"You picked: {Listprint(inv)}", 30, 500)
        delayprint("You walk over to a nearby table and pick up 2 small health potions!", 30, 500)
        delayprint("'You'd better hope that was the right choice...'", 30, 1500)
    End Sub
    Sub newmag()
        delayprint("Voice: 'You have done well to make it this far...", 30, 500)
        delayprint("Voice: 'I can bestow upon you some knowledge and a new spell'", 30, 500)
        delayprint("Voice: 'I will also increase your max mana and health...", 30, 500)
        maxphealth += 15
        maxpmana += 10
        phealth = maxphealth
        pmana = maxpmana
        delayprint($"Your max hp is now: {maxphealth} and your max mana is now: {maxpmana}", 30, 500)
        delayprint("Voice: 'It seems as though there is some other power still controlling the dungeon, although I am still here I can not stop these monsters until this being is defeated'", 30, 500)
        delayprint("Voice: 'I believe whatever has taken over this dungeon will be staying on the fifth floor of the dungeon'", 30, 500)
        delayprint("Voice: 'That is all I have found out for now'", 30, 500)
        delayprint("Voice: 'I can now give you a new, upgraded spell'", 30, 500)
        delayprint("Advanced Heal, Conjure Big Rock, Electric Ball, Firewall, Twister, Water Bolt", 30, 500)
        delayprint("Voice: 'So, what would you like?'", 30, 500)
        go1 = True
        While go1 = True
            Dim choice = Console.ReadLine().ToLower
            Select Case choice
                Case Is = "advanced heal"
                    maginv.Add("advanced heal")
                    go1 = False
                Case Is = "conjure big rock"
                    maginv.Add("conjure big rock")
                    go1 = False
                Case Is = "electric ball"
                    maginv.Add("electric ball")
                    go1 = False
                Case Is = "firewall"
                    maginv.Add("firewall")
                    go1 = False
                Case Is = "twister"
                    maginv.Add("twister")
                    go1 = False
                Case Is = "water bolt"
                    maginv.Add("water bolt")
                    go1 = False
            End Select
        End While
        delayprint($"You now have: {Listprint(maginv)}", 30, 500)
    End Sub
    Function Grabstats(ename As String)
        Dim jsonFile As String
        Using streamreader As New StreamReader("estats.json")
            jsonFile = streamreader.ReadToEnd()
        End Using
        Dim enemiesClass As enemies = JsonConvert.DeserializeObject(Of enemies)(jsonFile)
        Select Case ename.ToLower()
            Case "bigrat"
                Return enemiesClass.bigrat
            Case "biiigrat"
                Return enemiesClass.biiigrat
            Case "fenrir"
                Return enemiesClass.fenrir
            Case "anomaly"
                Return enemiesClass.anomaly
            Case "giantenemyspider"
                Return enemiesClass.giantenemyspider
            Case "hati"
                Return enemiesClass.hati
            Case "imp"
                Return enemiesClass.imp
            Case "koboldchief"
                Return enemiesClass.koboldchief
            Case "koboldguard"
                Return enemiesClass.koboldguard
            Case "koboldsoldier"
                Return enemiesClass.koboldsoldier
            Case "largerat"
                Return enemiesClass.largerat
            Case "lesserdemon"
                Return enemiesClass.lesserdemon
            Case "lesserimp"
                Return enemiesClass.lesserimp
            Case "lostsoul"
                Return enemiesClass.lostsoul
            Case "muffet"
                Return enemiesClass.muffet
            Case "muffetspet"
                Return enemiesClass.muffetspet
            Case "skoll"
                Return enemiesClass.skoll
            Case "smallenemyspider"
                Return enemiesClass.smallenemyspider
            Case Else
                Return enemiesClass.bigrat
        End Select
    End Function
    Sub Learnmag()
        delayprint("Voice: 'I'll tell you a little about myself...'", 30, 500)
        delayprint("Voice: 'I used to be the master of this dungeon... Against my own will of course.'", 30, 300)
        delayprint("Voice: 'Now that I am no longer controlled, I am able to destroy the dungeon...'", 30, 500)
        delayprint("Voice: 'Since nobody has been in this dungeon for a long time, I have nowhere near enough power to do anything.'", 30, 500)
        delayprint("Voice: 'Everytime you kill a monster here, I regain some of my power.'", 30, 500)
        delayprint("Voice: 'From those two rats that you defeated, I have regained enough power to grant you one spell, choose wisely':", 30, 500)
        delayprint("Basic Heal, Conjure Rock, Electric Shock, Fireball, Generate Wind, Water Ball", 30, 500)
        delayprint("Voice: 'So, what would you like?'", 30, 500)
        go1 = True
        While go1 = True
            Dim choice = Console.ReadLine().ToLower
            Select Case choice
                Case Is = "basic heal"
                    maginv.Add("basic heal")
                    go1 = False
                Case Is = "conjure rock"
                    maginv.Add("conjure rock")
                    go1 = False
                Case Is = "electric shock"
                    maginv.Add("electric shock")
                    go1 = False
                Case Is = "fireball"
                    maginv.Add("fireball")
                    go1 = False
                Case Is = "generate wind"
                    maginv.Add("generate wind")
                    go1 = False
                Case Is = "water ball"
                    maginv.Add("water ball")
                    go1 = False
            End Select
        End While
        delayprint($"So you have chosen {Listprint(maginv)}", 30, 500)
    End Sub
    Sub randbattle(floornum)
        Dim numtypeofenemy As Integer
        Select Case floornum '3 new types of enemy per floor except floor 1
            Case Is = 1
                numtypeofenemy = 2
            Case Is = 2
                numtypeofenemy = 5
            Case Is = 3
                numtypeofenemy = 8
            Case Is = 4
                numtypeofenemy = 11
            Case Is = 5
                numtypeofenemy = 11
        End Select
        Dim r As Random = New Random()
        Select Case r.Next(1, numtypeofenemy + 1)
            Case Is = 1
                Scenario(2, 0, "Big Rat")
                battle("BigRat")
            Case Is = 2
                Scenario(2, 0, "Large Rat")
                battle("LargeRat")
            Case Is = 3
                Scenario(2, 0, "Kobold Soldier")
                battle("Koboldsoldier")
            Case Is = 4
                Scenario(2, 0, "Kobold Archer")
                battle("KoboldArcher")
            Case Is = 5
                Scenario(2, 0, "Kobold Guard")
                battle("KoboldGuard")
            Case Is = 6
                Scenario(2, 0, "Small Enemy Spider")
                battle("SmallEnemySpider")
            Case Is = 7
                Scenario(2, 0, "Giant Enemy Spider")
                battle("GiantEnemySpider")
            Case Is = 8
                Scenario(2, 0, "Muffets Pet")
                battle("MuffetsPet")
            Case Is = 9
                Scenario(2, 0, "Lesser Imp")
                battle("LesserImp")
            Case Is = 10
                Scenario(2, 0, "Lost Soul")
                battle("LostSoul")
            Case Is = 11
                Scenario(2, 0, "Imp")
                battle("Imp")
        End Select
    End Sub
    Sub Main()
        Intro()
        Console.Clear()
        Scenario(1, 0, "Floor 1")
        Scenario(2, 0, "Big rat")
        battle("BigRat")
        Scenario(3, 15, "")
        Console.Clear()
        Scenario(2, 0, "Large Rat")
        battle("LargeRat")
        Scenario(3, 25, "")
        Scenario(8, 10, "")
        Scenario(5, 0, "")
        Loot(floor, 0, 0)
        Console.Clear()
        Learnmag()
        randbattle(floor)
        Scenario(3, 25, "")
        Scenario(8, 12, "")
        Scenario(5, 0, "")
        Loot(floor, 0, 0)
        Console.Clear()
        Scenario(4, 0, " Biiig Rat")
        battle("BiiigRat")
        Scenario(6, 0, "")
        Console.Clear()
        Scenario(7, 0, "")
        'floor 2 now :)
        floor = 2
        Scenario(1, 0, "Floor 2")
        Scenario(2, 0, "Kobold Soldier")
        battle("KoboldSoldier")
        Scenario(3, 15, "")
        randbattle(floor)
        Console.Clear()
        Scenario(5, 0, "")
        Loot(floor, 0, 0)
        randbattle(floor)
        Scenario(3, 15, "")
        Scenario(8, 14, "")
        Console.Clear()
        Scenario(2, 0, "Kobold Guard")
        battle("KoboldGuard")
        Scenario(3, 20, "")
        Scenario(8, 13, "")
        Scenario(5, 0, "")
        Loot(floor, 0, 0)
        Console.Clear()
        Scenario(4, 0, " Kobold Chief")
        battle("KoboldChief")
        Scenario(6, 0, "")
        Console.Clear()
        Scenario(7, 0, "")
        'floor 3 now
        floor = 3
        Scenario(1, 0, "Floor 3")
        newmag()
        Console.Clear()
        Scenario(2, 0, "Small Enemy Spider")
        battle("SmallEnemySpider")
        Scenario(3, 15, "")
        Scenario(8, 13, "")
        Scenario(2, 0, "Giant Enemy Spider")
        battle("GiantEnemySpider")
        Scenario(3, 15, "")
        Scenario(8, 16, "")
        Scenario(5, 0, "")
        Loot(floor, 0, 0)
        Console.Clear()
        randbattle(floor)
        Scenario(3, 24, "")
        Scenario(8, 12, "")
        Console.Clear()
        Scenario(2, 0, "Muffets Pet")
        battle("MuffetsPet")
        Scenario(3, 25, "")
        Scenario(8, 20, "")
        Scenario(4, 0, " Muffet")
        battle("Muffet")
        Scenario(6, 0, "")
        Console.Clear()
        Scenario(7, 0, "")
        'floor 4
        floor = 4
        Scenario(1, 0, "Floor 4")
        Scenario(2, 0, "Lesser Imp")
        battle("LesserImp")
        Scenario(3, 40, "")
        Console.Clear()
        randbattle(floor)
        Scenario(3, 20, "")
        Scenario(8, 16, "")
        Scenario(5, 0, "")
        Loot(floor, 0, 0)
        Console.Clear()
        Scenario(2, 0, "Lost Soul")
        battle("LostSoul")
        Scenario(3, 25, "")
        Scenario(8, 17, "")
        Console.Clear()
        Scenario(2, 0, "Imp")
        battle("Imp")
        Console.Clear()
        Scenario(5, 0, "")
        Loot(floor, 0, 0)
        Scenario(3, 15, "")
        Scenario(8, 20, "")
        Console.Clear()
        Scenario(4, 0, " Lesser Demon")
        battle("LesserDemon")
        Scenario(6, 0, "")
        Console.Clear()
        Scenario(7, 0, "")
        'final floor mostly custom stuff so new scenario stuff ig
        floor = 5
        Thread.Sleep(500)
        Console.Clear()
        Scenario(9, 0, "")
        Scenario(10, 0, "Hati")
        battle("Hati")
        Scenario(3, 50, "")
        Scenario(8, 15, "")
        Scenario(5, 0, "")
        Loot(floor, 0, 0)
        Thread.Sleep(500)
        Console.Clear()
        Scenario(10, 0, "Skoll")
        battle("Skoll")
        Scenario(3, 50, "")
        Scenario(8, 15, "")
        Scenario(5, 0, "")
        Loot(floor, 0, 0)
        Thread.Sleep(500)
        Console.Clear()
        Scenario(10, 0, "Fenrir")
        battle("Fenrir")
        Console.Clear()
        Scenario(3, 100, "")
        Scenario(8, 40, "")
        Scenario(11, 0, "")
        battle("anomaly")
        Scenario(12, 0, "")
    End Sub
End Module