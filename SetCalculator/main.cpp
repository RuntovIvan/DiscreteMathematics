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
	
	int count = GetInt("������� ���������� ��������: ", 3, 100);
	vector<Set> setVector = GetSetVector(count);
	
	bool isFinished = false;
	do
	{
		cout << endl << "�������:" << endl;
		cout << "1. ������� ���������" << endl;
		cout << "2. ������� ��������" << endl;
		cout << "3. ������ ���������" << endl;
		cout << "4. �����" << endl << endl;

		int command = GetInt("������� ����� �������: ", 1, 4);

		switch (command)
		{
		case 1:
			ShowSets("��������� ���������:", setVector);
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
		getline(cin, input); // ����� ������ �������

		istringstream ss(input); // ��������� �����

		if (ss >> value) // ���� ����� ����
		{
			string remaining; // ����������� ������
			if (ss >> remaining) // �������� �� ���-�� � ������
				cout << "������������ ����. ������� ����� �����." << endl << endl;

			else if (value < min || value > max)
				cout << "����� ������� �� ��������� (" << min << " ; " << max << ")." << endl << endl;

			else
				isParsed = true;
		}
		else
			cout << "������������ ����. ������� ����� �����." << endl << endl;

	} while (!isParsed);

	
	return value;
}

Set ManualFilling()
{
	Set set;
	int length = GetInt("������� ����� ���������: ", 0, 101);
	
	while (set.GetLength() < length)
	{
		int element = GetInt("������� ������� ��������� (�������� " + to_string(length - set.GetLength()) + "): ", -50, 50);

		if (set.Contains(element))
			cout << "��������� ��� �������� ������� " << element << endl << endl;
		else
		{
			set.InsertElement(element);
			cout << "������� " << element << " �������� � ���������" << endl << endl;
		}
	}
	return set;
}

Set RandomFilling()
{
	Set set;
	int length = GetInt("������� ����� ���������: ", 0, 101);
	
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
		cout << "�������:" << endl;
		cout << "1. �������������" << endl;
		cout << "2. �������������" << endl;
		cout << "3. ������" << endl;
		cout << "4. ��������" << endl;
		cout << "5. �������" << endl << endl;

		int command = GetInt("������� ����� �������: ", 1, 5);

		switch (command)
		{
		case 1:
			result = result.Intersection(AllPositive());
			cout << "������� 1 ������" << endl << endl;
			break;
		case 2:
			result = result.Intersection(AllNegative());
			cout << "������� 2 ������" << endl << endl;
			break;
		case 3:
			result = result.Intersection(AllEven());
			cout << "������� 3 ������" << endl << endl;
			break;
		case 4:
			result = result.Intersection(AllOdd());
			cout << "������� 4 ������" << endl << endl;
			break;
		case 5:
			int div = GetInt("������� ����� ���������: ", 1, 50);
			result = result.Intersection(AllMultiple(div));
			cout << "������� 5 ������" << endl << endl;
			break;
		}

		cout << "����� �� ������ ��� �������? (������� ������������.)" << endl;
		int answer = GetInt("������� 1 (�����) ��� 0 (�� �����): ", 0, 1);

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
		cout << endl << "�������� ������ ���������� ��������� " << i + 1 << endl;
		cout << "1. �������" << endl;
		cout << "2. ��������� ��������" << endl;
		cout << "3. �� �������" << endl << endl;

		int command = GetInt("������� ����� ������� ����������: ", 1, 3);

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
		cout << "��������� " << i + 1 << " ������" << endl << endl;
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
	cout << endl << "������� ��������:" << endl;
	cout << "1. �����������" << endl;
	cout << "2. �����������" << endl;
	cout << "3. ��������" << endl;
	cout << "4. �������������� ��������" << endl;
	cout << "5. ����������" << endl << endl;

	int command = GetInt("������� ����� ��������: ", 1, 5);

	switch (command)
	{
	case 1:
		DoubledOps("�����������", "�����������", '+', sets);
		break;
	case 2:
		DoubledOps("�����������", "�����������", '*', sets);
		break;
	case 3:
		DoubledOps("��������", "��������", '-', sets);
		break;
	case 4:
		DoubledOps("�������������� ��������", "�������������� ��������", '^', sets);
		break;
	case 5:
		ComplOp(sets);
		break;
	}
}

void ComplOp(vector<Set> sets)
{
	cout << endl << "���������� ���������:" << endl;

	int setNum = GetInt("������� ����� ���������: ", 1, sets.size());
	Set result = sets[setNum - 1].Complement();

	cout << endl << "����������� ���������:" << endl;
	cout << setNum << ". " << sets[setNum - 1] << endl;

	cout << endl << "��������� ����������:" << endl;
	cout << "!" << setNum << " = " << result << endl;
}

// ������� �� ������ �������� � 2 ����������, ���� ���� ����� (��� ��������)
void DoubledOps(string mes1, string mes2, char op, vector<Set> sets)
{
	cout << endl << mes1 << " ��������:" << endl;

	int setNum1 = GetInt("������� ����� ������� ���������: ", 1, sets.size());
	int setNum2 = GetInt("������� ����� ������� ���������: ", 1, sets.size());

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

	cout << endl << "����������� ���������:" << endl;
	cout << setNum1 << ". " << sets[setNum1 - 1] << endl << endl;
	cout << setNum2 << ". " << sets[setNum2 - 1] << endl;

	cout << endl << "��������� " << mes2 << ':' << endl;
	cout << setNum1 << ' ' << op << ' ' << setNum2 << " = " << result << endl;
}

void InputExpression(vector<Set> sets)
{
	cout << endl << "������ �� ����� ���������" << endl;
	cout << "� �������� ��������� ��������� ������ ��������" << endl;
	cout << "��������� ��������:" << endl;
	cout << "! (����������)" << endl;
	cout << "* (�����������)" << endl;
	cout << "+ (�����������)" << endl;
	cout << "- (��������)" << endl;
	cout << "^ (�������������� ��������)" << endl;

	cout << endl << "������� ���������: ";
	string expression;
	getline(cin, expression);

	try
	{
		Parser parser(sets);
		Set result = parser.Evaluate(expression);

		cout << endl << "���������: " << endl;
		cout << expression << " = " << result << endl;
	}

	catch (const exception& e)
	{
		cout << "������. " << e.what() << endl << endl;
	}
}