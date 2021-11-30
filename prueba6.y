Estado1	: Estado1 '+' Estado2
	| Estado2;

Estado2	: Estado2 '*' Estado3
	| Estado3;

Estado3	: '(' Estado1 ')'
	| 'primer terminal'
	| 'terminal prueba';
	
Estado4 : Estado5 | Estado6;

Estado5 : 'hola buenas tardes' | Estado6;




Estado6 : 'Otra prueba';