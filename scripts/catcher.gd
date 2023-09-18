extends Area2D

var ball: Ball
var hand: Hand

var _is_ball_entered := false


func _physics_process(_delta):
	if _is_ball_entered and !ball.is_flying():
		hand.catch(ball)
		queue_free()
	

func _on_body_entered(body):
	if body != ball:
		return
		
	_is_ball_entered = true
