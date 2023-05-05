# tsb_work_test_james_persaud
A semi-faithful asteroids clone for The Sandbox unity developer work test

The current state of this project is barely started in terms of the functionality however a lot of the underlying systems are in place, as close to a state in which I'm as happy with them as can be expected.

-----

You can currently only fly the player's ship around and shoot, with A and D to steer and P to thrust. O will fire bullets. There are some asteroids you can destroy but for now the player is invulnerable, there are no flying saucers and there is no score or game over condition.

I chose to shoot as close to the mark of the original game as possible so some things I've tried to be authentic with such as the size of the play area/ship, speed, acceleration and interia. Although these need a bit of tweaking I've made it easy to do so in the unity Editor while sticking to the ECS entities requirements. Turning is in 5 degree increments too like in the arcade.

A lot of systems have been written in compromised and sometimes not entirely safe ways just to get things done in the time constraints. I initially set out to
use jobs but ended up with simple ComponentSystem based systems for speed and simplicity. More parallelisation is an obvious improbement that could be made especially if the game needed to handle thousands of asteroids (perhaps in a vast scrolling world, certainly not on one small screen)

The implementation of events is rough and ready but I feel also quite in the spirit of ECS rather than relying on the usual C# events.

Collision detection is very basic and if I had more time I would base it on sprite masks.

There are additional notes on the design in the comments of SettingsComponent.cs, please don't miss those.

-----

Total time spent on this test is around 6 hours as of Friday evening 05/05/2023 Getting up to speed with ECS and in particular with the version available in the 2020 Unity build has been a major time sink.

But now that there's some momentum I hope to be able to add all of the other requested basic features soon and write up a little more about my approach.

James Persaud
