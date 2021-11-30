Estado1	: Estado1 '+' Estado2
	| Estado2

Estado2	: Estado2 '*' Estado3
	| Estado3

Estado3	: '(' Estado1 ')'
	| 'primer terminal'
	| 'terminal prueba'