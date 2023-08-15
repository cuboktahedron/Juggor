extends Node

var env = {}

@onready var _camera: Camera2D = $Camera2D
@onready var _left_hand: Hand = $LeftHand
@onready var _right_hand: Hand = $RightHand
@onready var _site_swap: Control = $CanvasLayer/SiteSwap


func _ready():
	_site_swap.env = env
	_site_swap.progressed.connect(_progressed_pattern)

	_left_hand.env = env
	_right_hand.env = env
	
	change_zoom(env.zoom)


func _physics_process(delta):
	var lh_dir = Vector2.ZERO
	if Input.is_action_pressed("lh_left"):
		lh_dir.x -= 1
	if Input.is_action_pressed("lh_right"):
		lh_dir.x += 1
	if Input.is_action_pressed("lh_up"):
		lh_dir.y -= 1
	if Input.is_action_pressed("lh_down"):
		lh_dir.y += 1
	var rh_dir = Vector2.ZERO
	if Input.is_action_pressed("rh_left"):
		rh_dir.x -= 1
	if Input.is_action_pressed("rh_right"):
		rh_dir.x += 1
	if Input.is_action_pressed("rh_up"):
		rh_dir.y -= 1
	if Input.is_action_pressed("rh_down"):
		rh_dir.y += 1
#	if Input.is_action_just_pressed("lh_throw"):
#		_throw(0, 9)
#	if Input.is_action_just_pressed("rh_throw"):
#		_throw(1, 7)

	_left_hand.position += lh_dir.normalized() * 400 * delta
	_right_hand.position += rh_dir.normalized() * 400 * delta


func set_pattern(patterns: Array):
	_site_swap.change_patterns(patterns)
	

func change_zoom(zoom: float):
	_camera.zoom = Vector2(1.0, 1.0) * zoom
	_camera.offset.x = 150 * (1.0 / zoom)
	_camera.offset.y = -224 * 1.0 / zoom


func _progressed_pattern(hand: int, pattern: int):
	_throw(hand, pattern)


func _throw(hand: int, pattern: int):
	if hand == 0:
		if pattern % 2 == 0:
			_left_hand.throw(_left_hand, pattern)
		else:
			_left_hand.throw(_right_hand, pattern)
	elif hand == 1:
		if pattern % 2 == 0:
			_right_hand.throw(_right_hand, pattern)
		else:
			_right_hand.throw(_left_hand, pattern)
