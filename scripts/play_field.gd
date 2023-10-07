extends Node

signal auto_zoomed(zoom: float)

const Hand = preload("res://scenes/hand.tscn")

var env = {}
var _hands = []

@onready var _camera: Camera2D = $Camera2D
@onready var _site_swap: Control = $CanvasLayer/SiteSwap

func _ready():
	_site_swap.env = env
	_site_swap.progressed.connect(_progressed_pattern)
	
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


func setup(hands: Array, env: Dictionary):
	var sp = env.siteswap_pattern
	var ball_colors = Ball.BASE_COLORS
	var ball_coloring = env.ball_coloring

	_setup_hands(hands)
	_setup_patterns(sp)
	_setup_balls(sp, ball_colors, ball_coloring)


func change_zoom(zoom: float):
	_camera.zoom = Vector2(1.0, 1.0) * zoom
	_camera.offset.x = 150 * (1.0 / zoom)
	_camera.offset.y = -224 * 1.0 / zoom
	
	for node in get_children():
		var ball = node as Ball
		if ball != null:
			ball.change_zoom(zoom, env.is_fix_size)
			continue
		
		var hand = node as Hand
		if hand != null:
			hand.change_zoom(zoom, env.is_fix_size)


func _setup_hands(hands: Array):
	
	var hand_no = 0
	for hand in hands:
		var new_hand := Hand.instantiate()
		new_hand.name = "Hand%s" % str(hand_no)
		new_hand.env = env
		new_hand.position = hand.position
		new_hand.hand_no = hand_no
		
		var path_phase = 0
		var hand_times = []
		for path in hand.paths:
			var hand_path_name = "%sPath%s" % [new_hand.name, str(path_phase)]
			var hand_path = HandPath.new(hand_path_name, path.time, path.points)
			hand_path.position = hand.position
			add_child(hand_path)
			new_hand.add_hand_path(hand_path)
			hand_times.push_back(path.time)
			path_phase += 1

		new_hand.hand_times = hand_times
		hand_no += 1
		
		_hands.push_back(new_hand)
		add_child(new_hand)


func _setup_patterns(sp: SiteswapPattern):
	_site_swap.change_patterns(sp)

	var zoom = _calculate_zoom(sp)
	auto_zoomed.emit(zoom)


func _setup_balls(
	sp: SiteswapPattern,
	ball_colors: Array,
	ball_coloring: Core.BallColoring):

	var rest_ball = sp.ball_num()
	print("rest_ball: %d" % rest_ball)
	
	var hand_no = 0
	var ball_nums := []
	var ball_no = 0
	ball_nums.resize(_hands.size())
	ball_nums.fill(0)
	
	var active_balls := []
	for i in _hands.size():
		active_balls.push_back([])
	active_balls = active_balls.map(func(a):
		a.resize(36)
		a.fill(0)
		return a)

	var loop_num := 0
	while true:
		loop_num += 1
		if loop_num == 1000:
			break

		for pattern in sp.patterns:
			if rest_ball == 0:
				return
			
			for i in active_balls.size():
				var ball_num = active_balls[i].pop_front()
				ball_nums[i] += ball_num
				active_balls[i].push_back(0)
			
			if pattern.height != 0 and ball_nums[hand_no] == 0:
				if ball_coloring == Core.BallColoring.BY_BALL_NO:
					if ball_colors.size() > ball_no:
						_hands[hand_no].add_ball(ball_colors[ball_no], ball_no)
					else:
						_hands[hand_no].add_ball(
							Ball.BASE_COLORS[pattern.height % Ball.BASE_COLORS.size()], ball_no)
				else:
					_hands[hand_no].add_ball(
						Ball.BASE_COLORS[pattern.height % Ball.BASE_COLORS.size()], ball_no)

				ball_no += 1
				rest_ball -= 1
				if pattern.height % 2 == 0:
					active_balls[hand_no][pattern.height] += 1
				else:
					active_balls[(hand_no + 1) % _hands.size()][pattern.height] += 1
				
			hand_no = (hand_no + 1) % _hands.size()
			
	if loop_num == 1000:
		push_error("ball setup is not ended")


func _calculate_zoom(sp: SiteswapPattern):
	var max_pattern = 0
	for pattern in sp.patterns:
		max_pattern = max(max_pattern, pattern.height)

	if max_pattern <= 2:
		return 1

	var base_time = max_pattern - 0.5
	var actual_time = base_time * env.tempo
	var v0 = (-0.5 * env.gravity * actual_time ** 2) / actual_time
	var t_hmax = -v0 / env.gravity
	var hmax = (v0 * t_hmax - 0.5 * env.gravity * t_hmax ** 2) * env.tempo
	
	print("t_hmax=%s" % t_hmax)
	print("hmax=%s" % hmax)

	var zoom = 1.0
	var i = 0
	while 1.0 / zoom * 648 < -hmax:
		zoom = 1.0 - i * 0.01 
		i += 1
	
	return zoom
	
	
func _progressed_pattern(hand: int, pattern: SiteswapFactor):
	_throw(hand, pattern)


func _throw(hand: int, pattern: SiteswapFactor):
	if hand == 0:
		if pattern.height % 2 == 0:
			_hands[hand].throw(_hands[0], pattern.height)
		else:
			_hands[hand].throw(_hands[1], pattern.height)
	elif hand == 1:
		if pattern.height % 2 == 0:
			_hands[hand].throw(_hands[1], pattern.height)
		else:
			_hands[hand].throw(_hands[0], pattern.height)
