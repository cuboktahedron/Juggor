class_name SiteswapPattern
extends Node

var patterns := []

# <pattern> ::= <factor>+
# <factor>  ::= <term> | "(" <term>x? "," <term>x? ")"
# <term>    ::= "0".."9" | "a".."z" | "A".."z"

func _init(patterns: Array):
	self.patterns = patterns
	

static func parse(context: String) -> SiteswapPattern:
	context = context.replace(" ", "").to_upper()
	
	var patterns = []
	for pattern in context.split(""):
		if ("A" <= pattern and pattern <= "Z") or \
			("0" <= pattern and pattern <= "9"):
			
			patterns.push_back(SiteswapFactor.parse(pattern))
		else:
			return null

	return SiteswapPattern.new(patterns)


func is_valid() -> bool:
	if patterns.is_empty():
		return false
	
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


func ball_num() -> int:
	if !is_valid():
		return 0
		
	var sum = 0
	for pattern in patterns:
		sum += pattern.height
	return sum / patterns.size()


func label() -> String:
	return "".join(patterns.map(func(pattern): return pattern.label()))
