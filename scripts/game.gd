extends Node

const PlayField = preload("res://scenes/play_field.tscn")

var env := {}
var current_play_field = null

@onready var _control_layer := $ControlLayer
@onready var _control_panel: ControlPanel = _control_layer.get_node("ControlPanel")


func _ready():
	env.gravity_scale = _control_panel.gravity
	env.gravity = 980.0 * env.gravity_scale
	env.time_scale = 1.0
	env.zoom = _control_panel.zoom
	env.tempo = 0.5
	env.patterns = [3]
	env.style = ThrowDefinitions.Default
	env.is_fix_size = false
	env.ball_coloring = Core.BallColoring.BY_PATTERN

	_reset_play_field()


func _reset_play_field():
	if current_play_field != null:
		current_play_field.queue_free()

	current_play_field = PlayField.instantiate()
	current_play_field.env = env
	current_play_field.auto_zoomed.connect(_on_play_field_auto_zoomed)
	add_child(current_play_field)

	var hands = ThrowDefinitions.Defs[env.style]
	current_play_field.setup(hands, env)


func _on_play_field_auto_zoomed(zoom: float):
	_control_panel.change_zoom(zoom)
	
	
func _on_patterns_change_pattern(patterns: Array):
	env.patterns = patterns
	_reset_play_field()


func _on_control_panel_gravity_changed(gravity: float):
	env.gravity_scale = gravity
	env.gravity = 980 * env.gravity_scale


func _on_control_panel_time_changed(time: float):
	env.time_scale = time
	Engine.set_time_scale(time)
	
	if time >= 1:
		Engine.physics_ticks_per_second = int(60 * time)
	else:
		Engine.physics_ticks_per_second = 60


func _on_control_panel_zoom_changed(zoom: float):
	env.zoom = zoom
	current_play_field.change_zoom(zoom)


func _on_styles_change_style(style):
	env.style = style
	_reset_play_field()


func _on_balls_ball_coloring_changed(coloring: Core.BallColoring):
	env.ball_coloring = coloring
	_reset_play_field()


func _on_balls_fix_size_changed(is_fix_size: bool):
	env.is_fix_size = is_fix_size
	current_play_field.change_zoom(env.zoom)
