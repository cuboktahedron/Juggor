extends Node


func _physics_process(_delta):
	if Input.is_action_just_pressed("pause"):
		get_tree().paused = !get_tree().paused
	
