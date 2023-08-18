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

	reset_play_field([3])


func reset_play_field(patterns: Array):
	if current_play_field != null:
		current_play_field.queue_free()

	current_play_field = PlayField.instantiate()
	current_play_field.env = env
	add_child(current_play_field)
	current_play_field.set_pattern(patterns)


func _on_patterns_change_pattern(patterns: Array):
	reset_play_field(patterns)


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


func _on_change_pattern_form_change_pattern(patterns: Array):
	reset_play_field(patterns)
