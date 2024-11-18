# Stats

## Description
A way to access an entities stats and edit them if needed

## New Stats I Purpose

Name | Battle | World
-|-|-
health | dmg till you die |
mana | how much spells you can cast | 
armor | physical resistance | 
strength | physical attack strength | athletism, lifting
constitution | physical and sickness resistance | endurance, torture
dexterity | accurasy, evasion, combat initiative | stealth, accuracy
intellect | effects certain magic attacks | problem-solving, trivia
wisdom | magic attack and curse resistance | perception, survival, common sense
charisma | effects certain magic attacks (think bard, or dancer) | shop prices, negotiations/persuasions, intimidation and deception

### Player Stats (Test to see progression)
||| Str
16
||| Con
17
||| Dex
14
||| Int
7
||| Wis
10
||| Cha
13
|||

Stat | 1 | 25 | 50 | 100
-|-|-|-|-
XP for level up | 16 | 93 | 283 | 999
xp from test enemy (7.6) | 3 | 6 | 11 | 23
xp from same stats enemy (12.83) | 3 | 10 | 19| 40
xp from tougher enemy (16) | 3 | 12 | 24 | 52
HP | 23 | 102 | 221 | 520
MP | 14 | 21 | 32 | 61
Con | 17 | 47 | 95 | 217
Dex | 14 | 39 | 78 | 178
Int | 7 | 19 | 38 | 89
Wis | 10 | 28 | 56 | 127
Cha | 13 | 36 | 73 | 166

### Enemy Stats
||| Str
12
||| Con
10
||| Dex
8
||| Int
5
||| Wis
4
||| Cha
7
|||

### Battle Sim at Lvl. 1

#### Player
23 HP
||| Hands
- 15 dmg
- 10 dmg if enemy had 5 armor
||| Sword with 3 dmg
- 18 dmg 
- 13 dmg w/ 0% peircing while enemy had 5 armor
- 15 dmg w/ 50% peircing while enemy has 5 armor
||| 

takes 1-2 attacks to kill enemy

earns 3xp per enemy killed (1-6, for 3-18)

15 xp required to level up. 6 of these enemies have to be killed. at low level, youll get always get about the same xp

#### Enemy
- 15 HP
- 11 base dmg agains player

takes 3 attacks to kill player

### Battle Sim at Lvl. 25

#### Player
102 HP
||| Hands
- 39 dmg
- 29 dmg if enemy had 10 armor
||| Sword with 9 dmg
- 48 dmg 
- 38 dmg w/ 0% peircing while enemy had 10 armor
- 43 dmg w/ 50% peircing while enemy has 10 armor
||| 

takes 2-3 attacks to kill enemy

earns 6xp per enemy killed (1-6, for 6-36)

88 xp required to level up. 16 of these enemies have to be killed. this is a weaker creature, so low xp. a stronger creature could yeild 9-12 xp at this level

#### Enemy
- 82 HP
- 21 base dmg agains player

takes 5 attacks to kill player

### Calculations
- health = 0.75 * level^1.3 + con + 5
- mana = wis * 0.4 + 10
- stats = (0.47 (stat / 20)) * level^1.35 + stat
- damage = str/int + weaponDMG - (enemyCon / 5 + enemyArmor(1 - weaponPeircing / 100))

if stats are too slow to increase at low levels (1-5), i found a way to change that, but i think itll be good slower (which is moving the whole graph to the left and readjusting for values)

xp is earned per character, so each character will get different amounts of xp, and no xp will be split/shared
- xp needed = 0.0895 (level + 5) + 12.8
- xp earned = 0.15 (level (enemyAveStats/playerAveStats))^1.2 + 3 + bonusXP

if leveling is too slow, should be able to raise the 0.15 to be a tad higher. bonusXP would be used for bosses or special enemies

And also, the formulas always give decimal values, so we will round to the smallest whole number in the game.

Site and project i used to calculate this stuff. Can easily change values and see results there: 
- stats - https://www.desmos.com/calculator/iiwdktkqlw
- leveling - https://www.desmos.com/calculator/sni0gj2hax
