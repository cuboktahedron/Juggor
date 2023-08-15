class_name Ball
extends RigidBody2D

var life_time: float


func _process(delta):
	life_time -= delta
	

func is_flying() -> bool:
	return life_time > 0.0
