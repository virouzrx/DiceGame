# Dice game project

> The degree of complexity increases with the amount of time spent on the project.

~Me, and probably many others as well

This project is an attempt to recreate a dice game known as Farkle or 10000. The goal is to create a fully functional game, with different kind of bots and simulations, fully tested and accessible through web.

### Rules: 

The goal is to score points based on specific combinations of dice rolls. The game has the following rules:

#### Rolling the Dice:

You roll six dice at the beginning.
Scoring Points:

A single 1 scores 10 points.

A single 5 scores 5 points.

Three or more dice of the same number score points based on the number shown on the dice:

Three 1s score 100 points (10 x 10).

Three 2s score 20 points (2 x 10).

Three 3s score 30 points (3 x 10).

Three 4s score 40 points (4 x 10).

Three 5s score 50 points (5 x 10).

Three 6s score 60 points (6 x 10).

If you roll four of the same number, the points are doubled:

Four 2s score 80 points (2 x 10 x 2).

Four 3s score 120 points (3 x 10 x 2), and so on.

For five of the same number, the points are quadrupled:

Five 3s score 180 points (3 x 10 x 2 x 2), and so on.

For six of the same number, the points are multiplied by eight:

Six 4s score 320 points (4 x 10 x 2 x 2 x 2), and so on.

Continuing the Game:

After each roll, you must set aside at least one scoring die before rolling the remaining dice again.
 choose to set aside more than one die, you do not need to set aside all scoring dice of a given type.
If at any point you roll the dice and no scoring dice are present, your turn ends, and you score no points for that round.
If you set aside all six dice that score points, you can pick up all six dice and continue rolling.
Example:

You roll: 3, 3, 3, 1, 2, 2.
You can choose to set aside the three 3s (30 points) or the single 1 (10 points).
If you set aside the three 3s, you will roll the remaining three dice.
Next roll: 5, 5, 6.
You can choose to set aside one 5 (5 points) or both 5s (10 points).

### Objective:

The objective is to reach 1000 points. Game is divided into 3 game phases. 

* Not entered

At the beginning of the game, non of the players entered the game. To enter a game, you need to throw 100 points. 

* Entered 

After throwing 100 or more points, player has entered a game. That allows the player to increase his score any time the player throws 30 or more points. If the player throw no dice that give points on the first throw (for example 2,2,3,3,6,6), player's score is reduced by 50 points. 

If the player overtakes another player in score, the overtaken player's score is reduced by 100. If, after overtaking, overtaken player is overtaken by yet another player, their score is reduced by 100 one more time, and so on. 

* Finishing  

After the player reaches 900 points, the player has to throw 100 points. At this point, player cannot increase his score anymore.

* Finished 

If the player reaches 1000 points, the game ends.

# Project state 

1. ~~Create functional game~~
2. Create bots with different rules and different risk management
    1. Create rules for bots that simulate real person's approach
        1. ~~No risk approach~~
        2. ~~Little risk approach~~
        3. Moderate risk approach 
        4. Risky approach
    2. Implement method allowing calculating probability of throwing a particular score 
       1. ~~Implement Monte Carlo approach~~
3. Create simulations to determine which strategy is the best
4. Move the game from console version to web version
5. Implement Redis for storing game state

