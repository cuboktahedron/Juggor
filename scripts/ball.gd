class_name Ball
extends RigidBody2D


@onready var mesh = $MeshInstance2D

const BASE_COLORS = [
	Color("#E60012"),
	Color("#8FC31F"),
	Color("#00A0E9"),
	Color("#920783"),
	Color("#F39800"),
	Color("#009944"),
	Color("#0068B7"),
	Color("#E5004F"),
	Color("#FFF100"),
	Color("#009E96"),
	Color("#1D2088"),
	Color("#E4007F"),
]

var life_time: float


func _process(delta):
	life_time -= delta


func is_flying() -> bool:
	return life_time > 0.0


func set_color_by_pattern(pattern):
	mesh.modulate = BASE_COLORS[pattern % BASE_COLORS.size()]


func set_color(color: Color):
	mesh.modulate = color


func change_zoom(zoom: float, is_fix_size: bool):
	if is_fix_size:
		mesh.scale = Vector2(30, 30) / zoom
	else:
		mesh.scale = Vector2(30, 30)
