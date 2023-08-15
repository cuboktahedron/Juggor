extends Area2D

var ball: Ball

var _is_ball_entered := false


func _physics_process(delta):
	if _is_ball_entered and !ball.is_flying():
		ball.queue_free()
		queue_free()
	

func _on_body_entered(body):
	if body != ball:
		return
		
	_is_ball_entered = true
