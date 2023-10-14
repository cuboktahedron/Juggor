class_name SiteswapPattern

var patterns := []
var is_sync: bool

# <pattern>      ::= <factor>+ | <sync_factor>+
# <factor>       ::= <height> | "[" <height>+ "]"
# <height>       ::= "0".."9" | "A".."z"
# <sync_factor>> ::= "(" <sync_factor_factor> "," <sync_factor_factor ")"
# <sync_factor_factor>
#                ::= <sync_height>x? | "[" (<sync_height>x?)+ "]"
# <sync_height>  ::= "2" | "4" .."8" | "A" | "C" .. "Y"

func _init(patterns: Array, is_sync: bool):
	self.patterns = patterns
	self.is_sync = is_sync
	

static func parse(siteswap: String) -> SiteswapPattern:
	var context = SiteswapParseContext.new(siteswap)
	if context.is_eoc():
		return null
	
	var is_sync := context.peek() == "("
	var patterns := []
	while !context.is_eoc():
		if is_sync:
			var factor := SiteswapSyncFactor.parse(context)
			if factor == null:
				return null
			else:
				patterns.push_back(factor)
		else:
			var factor := SiteswapFactor.parse(context)
			if factor == null:
				return null
			else:
				patterns.push_back(factor)

	return SiteswapPattern.new(patterns, is_sync)


func is_valid() -> bool:
	if patterns.is_empty():
		return false

	if is_sync:
		return _is_valid_sync()
	else:
		return _is_valid_async()


func ball_num() -> int:
	if !is_valid():
		return 0

	if is_sync:
		return _ball_num_sync()
	else:
		return _ball_num_async()


func label() -> String:
	return "".join(patterns.map(func(pattern): return pattern.label()))


func max_height() -> int:
	if !is_valid():
		return 0
	
	var temp_heights = []
	if is_sync:
		for pattern in patterns:
			temp_heights.push_back(pattern.height1)
			temp_heights.push_back(pattern.height2)
	else:
		temp_heights = patterns.map(func(x): return x.height)
		
	return temp_heights.max()

	
func _is_valid_sync() -> bool:
	var sum = 0
	var temp_heights = []
	for pattern in patterns:
		sum += pattern.height1
		sum += pattern.height2
		temp_heights.push_back(pattern.height1)
		temp_heights.push_back(pattern.height2)

	if sum % (patterns.size() * 2) != 0:
		return false

	var size = temp_heights.size()
	for i in range(0, size):
		for j in  range(i + 1, size):
			var a = (temp_heights[i] + i) % size
			var b = (temp_heights[j] + j) % size
			if a == b:
				return false

	return true
	

func _ball_num_sync() -> int:
	var sum = 0
	for pattern in patterns:
		sum += pattern.height1
		sum += pattern.height2
	return sum / (patterns.size() * 2)


func _is_valid_async() -> bool:
	var sum = 0
	for pattern in patterns:
		sum += pattern.height

	if sum % patterns.size() != 0:
		return false
	
	var size = patterns.size()
	for i in range(0, size):
		for j in  range(i + 1, size):
			var a = (patterns[i].height + i) % size
			var b = (patterns[j].height + j) % size
			if a == b:
				return false

	return true


func _ball_num_async() -> int:
	var sum = 0
	for pattern in patterns:
		sum += pattern.height
	return sum / patterns.size()
