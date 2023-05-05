# tsb_work_test_james_persaud
A semi-faithful asteroids clone for The Sandbox unity developer work test

The current state of this project is barely started in terms of the functionality however a lot of the underlying systems are in place, as close to a state in which I'm as happy with them as can be expected.

-----

You can currently only fly the player's ship around, with A and D to steer and P to thrust.

I chose to shoot as close to the mark of the original game as possible so some things I've tried to be authentic with such as the size of the play area/ship, speed, acceleration and interia. Although these need a bit of tweaking I've made it easy to do so in the unity Editor while sticking to the ECS entities requirements. Turning is in 5 degree increments too like in the arcade.

There are additional notes on the design in the comments of SettingsComponent.cs, please don't miss those.

-----

Total time spent on this test is around 4 hours as of Friday afternoon 05/05/2023 Getting up to speed with ECS and in particular with the version available in the 2020 Unity build has been a major time sink.

But now that there's some momentum I hope to be able to add all of the other requested features soon.

James Persaud
