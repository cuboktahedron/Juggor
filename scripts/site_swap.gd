extends Control

signal progressed(hand: int, pattern: int)

var env := {}

var _patterns := []
var _time := 0.5 # delay for beginning
var _hand_num := 2
var _current := -1
var _current_hand := 0
var _ball_num:int


func _physics_process(delta):
	if _patterns.is_empty():
		return
	
	_time += delta * 1.0 / env.tempo
	
	if _time < 1.0:
		return

	_current += 1
	_current %= _patterns.size()

	progressed.emit(_current_hand, _patterns[_current])
	_current_hand += 1
	_current_hand %= _hand_num
	_time = _time - 1.0
	
	_refresh()


func change_patterns(new_patterns: Array) -> bool:
	if !_is_valid_patterns(new_patterns):
		return false
		
	_patterns = new_patterns
	
	for child in get_children():
		child.queue_free();

	for pattern in _patterns:
		var label = Label.new()
		label.add_theme_font_size_override("font_size", 32)
		label.text = str(pattern)
		add_child(label)

	_ball_num = _calc_ball_num(_patterns)

	return true


func _calc_ball_num(patterns):
	var sum = 0
	for pattern in patterns:
		sum += pattern
	return sum / patterns.size()


func _is_valid_patterns(patterns):
	return true


func _refresh():
	var index = 0
	for child in get_children():
		child = child as Label
		if index == _current:
			child.add_theme_color_override("font_color", Color(1, 0, 0))
		else:
			child.remove_theme_color_override("font_color")
		index += 1
