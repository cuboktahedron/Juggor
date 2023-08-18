class_name ControlPanel
extends Panel

signal gravity_changed(float)
signal time_changed(float)
signal zoom_changed(float)

var gravity: float:
	get:
		return _gravity_slider.value

var time: float:
	get:
		return _time_slider.value

var zoom: float:
	get:
		return _zoom_slider.value

@onready var _gravity_label := $Margin/VBox/Gravity/Label
@onready var _gravity_slider := $Margin/VBox/Gravity/HSlider
@onready var _time_label := $Margin/VBox/Time/Label
@onready var _time_slider := $Margin/VBox/Time/HSlider
@onready var _zoom_label := $Margin/VBox/Zoom/Label
@onready var _zoom_slider := $Margin/VBox/Zoom/HSlider


func _on_gravity_changed(_gravity):
	_gravity_label.text = "gravity: %s" % str(gravity)
	gravity_changed.emit(gravity)


func _on_time_changed(_time):
	_time_label.text = "time: %s" % str(time)
	time_changed.emit(time)


func _on_zoom_changed(_zoom):
	_zoom_label.text = "zoom: %s" % str(zoom)
	zoom_changed.emit(zoom)
