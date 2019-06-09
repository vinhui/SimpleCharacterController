# Simple Character Controller
This basic character controller uses the new input system package. This combined with the built-in CharacterController makes it really easy to create a first-person character controller.

This repo mainly consists of two components. A motor and an input handler. Splitting these two up makes it easier to for example create an AI that is bound by the same restrictions as a player.

This project was made using Unity 2019.2.0b5 but you can use the `CharacterControllerMotor` in any version. The `CharacterControllerInput` script relies on the `Input System 0.2.10` package, but you can easily change it to use the `Input` class.