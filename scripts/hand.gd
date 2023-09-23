class_name Hand
extends Area2D

const Ball = preload("res://scenes/ball.tscn")
const Catcher = preload("res://scenes/catcher.tscn")

var env = {}
var hand_no: int
var hand_times: Array

var _phase = 0
var _time = 0
var _hand_paths := []
var _balls := []

@onready var _play_field = get_parent()
@onready var _hand_positions := [
	get_node("../%sPath0/%sPath0PathFollow" % [name, name]),
	get_node("../%sPath1/%sPath1PathFollow" % [name, name])	
]

func _ready():
	_phase = hand_no

	if hand_no == 1:
		_time = 0.5


func _process(delta):
	_time += delta * 1.0 / env.tempo
	
	var hand_speed = 1.0 / hand_times[_phase]
	_hand_positions[_phase].progress_ratio = _time * hand_speed
	position = _hand_positions[_phase].global_position

	if _hand_positions[_phase].progress_ratio >= 1.0:
		_hand_positions[_phase].progress_ratio = 0.0
		_time = _time - hand_times[_phase]
		_phase = (_phase + 1) % _hand_positions.size()


func add_hand_path(hand_path: HandPath):
	_hand_paths.push_back(hand_path)


func throw(to: Hand, pattern: int):
	if pattern == 0 or pattern == 2:
		return

	var base_time = pattern - 0.5
	var actual_time = base_time * env.tempo
	if _balls.is_empty():
		push_warning("[%s] has no ball to throw" % name)
		return

	var ball: Ball = _balls.pop_front()
	remove_child(ball)

	var diff_pos = to._catch_position(to, base_time) - position;
	var impulse = Vector2.ZERO

	impulse.x = diff_pos.x / actual_time
	impulse.y = (diff_pos.y - 0.5 * env.gravity * actual_time ** 2) / actual_time

	_play_field.add_child(ball)
	ball.position = position
	ball.linear_velocity = Vector2.ZERO
	ball.gravity_scale = env.gravity_scale
	ball.life_time = actual_time - 0.05
	ball.freeze = false
	ball.apply_impulse(impulse)
#	ball.set_color_by_pattern(pattern)

	var catcher = Catcher.instantiate()
	catcher.position = Vector2.ZERO
	catcher.ball = ball
	catcher.hand = to
	to.add_child(catcher)


func add_ball(color: Color, ball_no: int):
	var ball: Ball = Ball.instantiate()
	ball.name = "Ball%s" % ball_no
	ball.position = Vector2.ZERO
	ball.freeze = true
	add_child(ball)
	ball.set_color(color)
	_balls.push_back(ball)


func _catch_position(to: Hand, target_time: float) -> Vector2:
	var work_phase = _phase
	var rest_time = target_time - (hand_times[work_phase] - _time)
	
	while rest_time > 0:
		work_phase = (work_phase + 1) % _hand_positions.size()
		rest_time -= hand_times[work_phase]
		
	var t = hand_times[work_phase] + rest_time

	var cur_progress_ratio = to._hand_positions[work_phase].progress_ratio
	to._hand_positions[work_phase].progress_ratio = t / hand_times[work_phase]
	var pos = to._hand_positions[work_phase].global_position
	to._hand_positions[work_phase].progress_ratio = cur_progress_ratio

	return pos


func catch(ball: Ball):
	ball.get_parent().remove_child(ball)
	print("[%s] catched ball(%s)" % [name, ball.name])
	add_child(ball)
	ball.position = position
	_balls.push_back(ball)
