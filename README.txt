# tsb_work_test_james_persaud
A semi-faithful asteroids clone for The Sandbox unity developer work test

The current state of this project is barely started in terms of the functionality however a lot of the underlying systems are in place, as close to a state in which I'm as happy with them as can be expected.

-----

Current features
 - Player ship and controlls: A and D to steer and P to thrust. O will fire bullets.
 - Asteroids
    can be shot
	will break down into smaller ones
 - Flying saucers (only the big saucer AI is implemented and no diagonal movement at this point)
	can be shot
	will fire bullets

I chose to shoot as close to the mark of the original game as possible in terms of aesthetics and mechanics so some things I've tried to be authentic with such as the size of the play area/ship, speed, acceleration and interia. Although these need a bit of tweaking I've made it easy to do so in the unity Editor while sticking to the ECS entities requirements. Turning is in 5 degree increments too like in the arcade.

A lot of systems have been written in compromised and sometimes not entirely safe ways just to get things done in the time constraints. I initially set out to
use jobs but ended up with simple ComponentSystem based systems for speed and simplicity. More parallelisation is an obvious improvement that could be made especially if the game needed to handle thousands of asteroids (perhaps in a vast scrolling world, certainly not on one small screen)

The implementation of events is rough and ready but I feel also quite in the spirit of ECS rather than relying on the usual C# events. The biggest weakness of this approach as implemented is that the events can only have one subscriber. I have researched some better approaches to events in ECS but nothing there seems worth doing given the scope of the assignment and the time available.

Collision detection is very basic and if I had more time I would base it on sprite masks.

There are additional notes on the design in the comments of GameSettingsComponent.cs, please don't miss those.

Would love to be able to add polish like sound effects and animations but not before the game is functionally complete.

-----

Total time spent on this test is around 8 hours as of Sunday evening 07/05/2023 Getting up to speed with ECS and in particular with the version available in the 2020 Unity build has been a major time sink.

Overall I feel as though this assignment would require un unreasonable amount of time, for unpaid work on top of an already full schedule, if one was to both complete all of the functionality listed and do things exactly as they would in a production environment.

James Persaud
