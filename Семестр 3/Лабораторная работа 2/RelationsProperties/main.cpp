#include "Relationship.h"
using namespace std;

int main()
{
	setlocale(LC_ALL, "Ru");
	
	Relationship rel;
	rel.ShowMatrix();

	if (rel.IsReflexive()) cout << "Рефлексивное" << endl;

	if (rel.IsAntireflexive()) cout << "Антирефлексивное" << endl;

	if (rel.IsSymmetric()) cout << "Симметричное" << endl;

	if (rel.IsAsymmetric()) cout << "Асимметричное" << endl;

	if (rel.IsAntisymmetric()) cout << "Антисимметричное" << endl;

	if (rel.IsTransitive()) cout << "Транзитивное" << endl;

	if (rel.IsConnective()) cout << "Связное" << endl;

	if (rel.IsEquivalence())
		cout << "Отношение эквивалентности" << endl;

	if (rel.IsStrictCompleteOrder())
		cout << "Строгий полный порядок" << endl;

	else if (rel.IsStrictPartialOrder())
		cout << "Строгий частичный порядок" << endl;

	else if (rel.IsNonstrictCompleteOrder())
		cout << "Нестрогий полный порядок" << endl;

	else if (rel.IsNonstrictPartialOrder())
		cout << "Нестрогий частичный порядок" << endl;
}