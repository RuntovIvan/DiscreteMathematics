#include "Set.h"
#include "Parser.h"
#include <limits>
#include <string>
#include <algorithm>
#include <sstream>
using namespace std;

int GetInt(string message, int min, int max);

Set GetUniversum();
Set AllPositive();
Set AllNegative();
Set AllEven();
Set AllOdd();
Set AllMultiple(int value);

Set ManualFilling();
Set RandomFilling();
Set ConditionalFilling();
vector<Set> GetSetVector(int count);
void ShowSets(string message, vector<Set> sets);

void SimpleOperation(vector<Set> sets);
void ComplOp(vector<Set> sets);
void DoubledOps(string mes1, string mes2, char op, vector<Set> sets);

void InputExpression(vector<Set> sets);

int main()
{
	setlocale(LC_ALL, "RU");
	srand(time(nullptr));
	
	int count = GetInt("Введите количество множеств: ", 3, 100);
	vector<Set> setVector = GetSetVector(count);
	
	bool isFinished = false;
	do
	{
		cout << endl << "Команды:" << endl;
		cout << "1. Вывести множества" << endl;
		cout << "2. Простые операции" << endl;
		cout << "3. Ввести выражение" << endl;
		cout << "4. Выйти" << endl << endl;

		int command = GetInt("Введите номер команды: ", 1, 4);

		switch (command)
		{
		case 1:
			ShowSets("Доступные множества:", setVector);
			break;
		case 2:
			SimpleOperation(setVector);
			break;
		case 3:
			InputExpression(setVector);
			break;
		case 4:
			isFinished = true;
			break;
		}
	} while (!isFinished);
	
	return 0;
}

Set GetUniversum()
{
	Set universum;
	for (int i = -50; i <= 50; i++)
		universum.InsertElement(i);
	return universum;
}

Set AllPositive()
{
	Set result;
	for (int i = 0; i <= 50; i++)
		result.InsertElement(i);
	return result;
}

Set AllNegative()
{
	Set result;
	for (int i = -50; i < 0; i++)
		result.InsertElement(i);
	return result;
}

Set AllEven()
{
	Set result;
	for (int i = -50; i <= 50; i++)
		if (i % 2 == 0)
			result.InsertElement(i);
	return result;
}

Set AllOdd()
{
	Set result;
	for (int i = -50; i <= 50; i++)
		if (i % 2 != 0)
			result.InsertElement(i);
	return result;
}

Set AllMultiple(int value)
{
	Set result;
	for (int i = -50; i <= 50; i++)
		if (i % value == 0)
			result.InsertElement(i);
	return result;
}

int GetInt(string message, int min, int max)
{
	bool isParsed = false;
	int value;
	string input;

	do
	{
		cout << message;
		getline(cin, input); // берем строку целиком

		istringstream ss(input); // строковый поток

		if (ss >> value) // если число есть
		{
			string remaining; // оаставшаяся строка
			if (ss >> remaining) // осталось ли что-то в строке
				cout << "Некорректный ввод. Введите целое число." << endl << endl;

			else if (value < min || value > max)
				cout << "Число выходит из диапазона (" << min << " ; " << max << ")." << endl << endl;

			else
				isParsed = true;
		}
		else
			cout << "Некорректный ввод. Введите целое число." << endl << endl;

	} while (!isParsed);

	
	return value;
}

Set ManualFilling()
{
	Set set;
	int length = GetInt("Введите длину множества: ", 0, 101);
	
	while (set.GetLength() < length)
	{
		int element = GetInt("Введите элемент множества (осталось " + to_string(length - set.GetLength()) + "): ", -50, 50);

		if (set.Contains(element))
			cout << "Множество уже содержит элемент " << element << endl << endl;
		else
		{
			set.InsertElement(element);
			cout << "Элемент " << element << " добавлен в множество" << endl << endl;
		}
	}
	return set;
}

Set RandomFilling()
{
	Set set;
	int length = GetInt("Введите длину множества: ", 0, 101);
	
	while (set.GetLength() < length)
		set.InsertElement(rand() % 101 - 50);

	return set;
}

Set ConditionalFilling()
{
	Set result = GetUniversum();
	bool isFinished = false;

	do
	{
		cout << "Условия:" << endl;
		cout << "1. Положительные" << endl;
		cout << "2. Отрицательные" << endl;
		cout << "3. Четные" << endl;
		cout << "4. Нечетные" << endl;
		cout << "5. Кратные" << endl << endl;

		int command = GetInt("Введите номер условия: ", 1, 5);

		switch (command)
		{
		case 1:
			result = result.Intersection(AllPositive());
			cout << "Условие 1 учтено" << endl << endl;
			break;
		case 2:
			result = result.Intersection(AllNegative());
			cout << "Условие 2 учтено" << endl << endl;
			break;
		case 3:
			result = result.Intersection(AllEven());
			cout << "Условие 3 учтено" << endl << endl;
			break;
		case 4:
			result = result.Intersection(AllOdd());
			cout << "Условие 4 учтено" << endl << endl;
			break;
		case 5:
			int div = GetInt("Введите число кратности: ", 1, 50);
			result = result.Intersection(AllMultiple(div));
			cout << "Условие 5 учтено" << endl << endl;
			break;
		}

		cout << "Нужно ли учесть еще условие? (Условия пересекаются.)" << endl;
		int answer = GetInt("Введите 1 (нужно) или 0 (не нужно): ", 0, 1);

		if (!answer)
			isFinished = true;

	} while (!isFinished);

	return result;
}

vector<Set> GetSetVector(int count)
{
	vector<Set> sets(count);

	for (int i = 0; i < count; i++)
	{
		cout << endl << "Выберите способ заполнения множества " << i + 1 << endl;
		cout << "1. Вручную" << endl;
		cout << "2. Случайные значения" << endl;
		cout << "3. По условию" << endl << endl;

		int command = GetInt("Введите номер способа заполнения: ", 1, 3);

		switch (command)
		{
		case 1:
			sets[i] = ManualFilling();
			break;
		case 2:
			sets[i] = RandomFilling();
			break;
		case 3:
			sets[i] = ConditionalFilling();
			break;
		}
		cout << "Множество " << i + 1 << " задано" << endl << endl;
	}

	return sets;
}

void ShowSets(string message, vector<Set> sets)
{
	cout << message << endl;
	for (int i = 0; i < sets.size(); i++)
	{
		cout << i + 1 << ". " << sets[i] << endl << endl;
	}
	cout << endl;
}

void SimpleOperation(vector<Set> sets)
{
	cout << endl << "Простые операции:" << endl;
	cout << "1. Объединение" << endl;
	cout << "2. Пересечение" << endl;
	cout << "3. Разность" << endl;
	cout << "4. Симметрическая разность" << endl;
	cout << "5. Дополнение" << endl << endl;

	int command = GetInt("Введите номер операции: ", 1, 5);

	switch (command)
	{
	case 1:
		DoubledOps("Объединение", "объединения", '+', sets);
		break;
	case 2:
		DoubledOps("Пересечение", "пересечения", '*', sets);
		break;
	case 3:
		DoubledOps("Разность", "разности", '-', sets);
		break;
	case 4:
		DoubledOps("Симметрическая разность", "симметрической разности", '^', sets);
		break;
	case 5:
		ComplOp(sets);
		break;
	}
}

void ComplOp(vector<Set> sets)
{
	cout << endl << "Дополнение множества:" << endl;

	int setNum = GetInt("Введите номер множества: ", 1, sets.size());
	Set result = sets[setNum - 1].Complement();

	cout << endl << "Участвующее множество:" << endl;
	cout << setNum << ". " << sets[setNum - 1] << endl;

	cout << endl << "Результат дополнения:" << endl;
	cout << "!" << setNum << " = " << result << endl;
}

// функция по замене операций с 2 операндами, если надо будет (под вопросом)
void DoubledOps(string mes1, string mes2, char op, vector<Set> sets)
{
	cout << endl << mes1 << " множеств:" << endl;

	int setNum1 = GetInt("Введите номер первого множества: ", 1, sets.size());
	int setNum2 = GetInt("Введите номер второго множества: ", 1, sets.size());

	Set result;
	switch (op)
	{
	case '+':
		result = sets[setNum1 - 1].Union(sets[setNum2 - 1]);
		break;
	case '*':
		result = sets[setNum1 - 1].Intersection(sets[setNum2 - 1]);
		break;
	case '-':
		result = sets[setNum1 - 1].Difference(sets[setNum2 - 1]);
		break;
	case '^':
		result = sets[setNum1 - 1].SymmetricDifference(sets[setNum2 - 1]);
		break;
	}

	cout << endl << "Участвующие множества:" << endl;
	cout << setNum1 << ". " << sets[setNum1 - 1] << endl << endl;
	cout << setNum2 << ". " << sets[setNum2 - 1] << endl;

	cout << endl << "Результат " << mes2 << ':' << endl;
	cout << setNum1 << ' ' << op << ' ' << setNum2 << " = " << result << endl;
}

void InputExpression(vector<Set> sets)
{
	cout << endl << "Сводка по вводу выражений" << endl;
	cout << "В качестве операндов выступают номера множеств" << endl;
	cout << "Доступные операции:" << endl;
	cout << "! (дополнение)" << endl;
	cout << "* (пересечение)" << endl;
	cout << "+ (объединение)" << endl;
	cout << "- (разность)" << endl;
	cout << "^ (симметрическая разность)" << endl;

	cout << endl << "Введите выражение: ";
	string expression;
	getline(cin, expression);

	try
	{
		Parser parser(sets);
		Set result = parser.Evaluate(expression);

		cout << endl << "Результат: " << endl;
		cout << expression << " = " << result << endl;
	}

	catch (const exception& e)
	{
		cout << "Ошибка. " << e.what() << endl << endl;
	}
}