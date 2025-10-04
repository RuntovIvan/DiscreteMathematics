#pragma once
#include "Set.h"
#include <vector>
#include <stack>
#include <map>
#include <sstream>
using namespace std;

class Parser
{
private:
	vector<Set> sets; // вектор всех множеств
	map<char, int> precedence; // приоритеты операций

	bool IsOperator(char c); // проверка, является ли оператором
	string InfixToPostfix(const string& infix); // преобразование инфиксной записи в постфиксную
	Set EvaluatePostfix(const string& postfix); // вычисление значения постфиксного выражения
	void CheckExpression(const string& expression); // проверка выражения на корректность

public:
	Parser(const vector<Set>& sets); // консруктор
	Set Evaluate(const string& expression); // вычисление значения выражения
};

Parser::Parser(const vector<Set>& sets) : sets(sets)
{
	precedence['!'] = 4; // дополненние
	precedence['*'] = 3; // пересечение
	precedence['+'] = 2; // объединение
	precedence['-'] = 1; // разность
	precedence['^'] = 1; // симметрическая разность
	precedence['('] = 0;
	precedence[')'] = 0;
}

bool Parser::IsOperator(char c)
{
	return c == '!' || c == '*' || c == '+' || c == '-' || c == '^';
}

string Parser::InfixToPostfix(const string& infix)
{
	string postfix = ""; // постфиксная строка
	stack<char> operators; // стек операторов

	for (int i = 0; i < infix.length(); i++)
	{
		char c = infix[i];

		if (isspace(c)) continue; // если символ пробел

		if (isdigit(c)) // если встретилась цифра
		{
			string number;
			while (i < infix.length() && isdigit(infix[i]))
			{
				number += infix[i]; // добавляет все цифры в числе
				i++;
			}
			i--;

			postfix += number + " "; // номер записывается в результирующую строку
		}
		else if (c == '!')
			operators.push(c); // операция ! помещается в стек

		else if (c == '(')
			operators.push(c); // '(' помещается в стек

		else if (c == ')')
		{
			// записываются все операторы до '('
			while (!operators.empty() && operators.top() != '(')
			{
				postfix += operators.top(); // записывается оператор из стека
				postfix += " ";
				operators.pop(); // оператор убирается из стека
			}

			if (!operators.empty() && operators.top() == '(')
				operators.pop(); // из стека удаляется '('
		}

		else if (IsOperator(c)) // бинарные операции
		{
			// записываются все операторы из стека с большим или равным приоритетом
			while (!operators.empty() && precedence[operators.top()] >= precedence[c])
			{
				postfix += operators.top();
				postfix += " ";
				operators.pop();
			}

			operators.push(c); // добавляется текущий оператор
		}

		else throw invalid_argument("Неизвестный символ: " + string(1, c));
	}

	// запись всех оставшихся операторов
	while (!operators.empty())
	{
		postfix += operators.top();
		postfix += " ";
		operators.pop();
	}

	return postfix;
}

Set Parser::EvaluatePostfix(const string& postfix)
{
	stack<Set> values; // стек с участвующими множествами
	istringstream pfx(postfix); // строковый поток на постфиксную запись
	string token; // операнд или оператор

	while (pfx >> token) // пока читается из постфикса
	{
		if (isdigit(token[0])) // если номер множества
		{
			int setNum = stoi(token);
			values.push(sets[setNum - 1]); // множество в стек
		}

		else if (token == "!")
		{
			Set operand = values.top(); // берем верхний операнд
			values.pop();
			values.push(operand.Complement()); // добавляем дополнение в стек
		}

		else // обработка бинарных операторов
		{
			Set right = values.top(); // правый операнд
			values.pop();
			Set left = values.top(); //левый операнд
			values.pop();


			switch (token[0])
			{
			case '+':
				values.push(left.Union(right)); // в стек добавляется объединение
				break;
			case '*':
				values.push(left.Intersection(right)); // в стек добавляется пересечение
				break;
			case '-':
				values.push(left.Difference(right)); // в стек добавляется разность
				break;
			case '^':
				values.push(left.SymmetricDifference(right)); // в стек добавляется симметрическая разность
				break;
			}
		}
	}

	// проверка результата
	if (values.size() != 1)
		throw invalid_argument("Некорректное выражение");

	return values.top(); // возвращаем результат выражения
}

Set Parser::Evaluate(const string& expression)
{
	CheckExpression(expression);

	string postfix = InfixToPostfix(expression); // преобразуем выражение в постфиксную запись
	return EvaluatePostfix(postfix);
}

void Parser::CheckExpression(const string& expression)
{
	if (expression == "")
		throw invalid_argument("Выражение не может быть пустым");

	int bracetBal = 0;
	bool expectOperand = true; // true - ожидается операнд, false - ожидается оператор

	for (int i = 0; i < expression.length(); i++)
	{
		char c = expression[i];
		if (isspace(c)) continue;

		if (expectOperand == true) // если ожидается операнд
		{
			if (isdigit(c))
			{
				string number;
				while (i < expression.length() && isdigit(expression[i]))
				{
					number += expression[i]; // добавляет все цифры в числе
					i++;
				}
				i--;

				int setNum = stoi(number);
				if (!(setNum >= 1 && setNum <= sets.size()))
					throw invalid_argument("Неверный номер множества: " + number);
				
				expectOperand = false; // теперь ожидается оператор
			}
			
			else if (c == '!')
				expectOperand = true; // после ! необходим оператор

			else if (c == '(')
			{
				bracetBal++; // баланс скобок +1
				expectOperand = true; // после ( необходим оператор
			}

			else
				throw invalid_argument("Некорректное выражение");
		}

		else // ожидается бинарный оператор
		{
			if (IsOperator(c) && c != '!')
				expectOperand = true; // после бинарной операции ожидается операнд

			else if (c == ')')
			{
				bracetBal--;
				if (bracetBal < 0)
					throw invalid_argument("Лишняя закрывающая скобка");

				expectOperand = false; // после ) требуется оператор
			}

			else
				throw invalid_argument("Некорректное выражение");
		}
		
	}

	if (expectOperand)
		throw invalid_argument("Некорректное выражение");

	if (bracetBal > 0)
		throw invalid_argument("Незакрытая скобка");
}