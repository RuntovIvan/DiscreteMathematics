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
	vector<Set> sets; // ������ ���� ��������
	map<char, int> precedence; // ���������� ��������

	bool IsOperator(char c); // ��������, �������� �� ����������
	string InfixToPostfix(const string& infix); // �������������� ��������� ������ � �����������
	Set EvaluatePostfix(const string& postfix); // ���������� �������� ������������ ���������
	void CheckExpression(const string& expression); // �������� ��������� �� ������������

public:
	Parser(const vector<Set>& sets); // ����������
	Set Evaluate(const string& expression); // ���������� �������� ���������
};

Parser::Parser(const vector<Set>& sets) : sets(sets)
{
	precedence['!'] = 4; // �����������
	precedence['*'] = 3; // �����������
	precedence['+'] = 2; // �����������
	precedence['-'] = 1; // ��������
	precedence['^'] = 1; // �������������� ��������
	precedence['('] = 0;
	precedence[')'] = 0;
}

bool Parser::IsOperator(char c)
{
	return c == '!' || c == '*' || c == '+' || c == '-' || c == '^';
}

string Parser::InfixToPostfix(const string& infix)
{
	string postfix = ""; // ����������� ������
	stack<char> operators; // ���� ����������

	for (int i = 0; i < infix.length(); i++)
	{
		char c = infix[i];

		if (isspace(c)) continue; // ���� ������ ������

		if (isdigit(c)) // ���� ����������� �����
		{
			string number;
			while (i < infix.length() && isdigit(infix[i]))
			{
				number += infix[i]; // ��������� ��� ����� � �����
				i++;
			}
			i--;

			postfix += number + " "; // ����� ������������ � �������������� ������
		}
		else if (c == '!')
			operators.push(c); // �������� ! ���������� � ����

		else if (c == '(')
			operators.push(c); // '(' ���������� � ����

		else if (c == ')')
		{
			// ������������ ��� ��������� �� '('
			while (!operators.empty() && operators.top() != '(')
			{
				postfix += operators.top(); // ������������ �������� �� �����
				postfix += " ";
				operators.pop(); // �������� ��������� �� �����
			}

			if (!operators.empty() && operators.top() == '(')
				operators.pop(); // �� ����� ��������� '('
		}

		else if (IsOperator(c)) // �������� ��������
		{
			// ������������ ��� ��������� �� ����� � ������� ��� ������ �����������
			while (!operators.empty() && precedence[operators.top()] >= precedence[c])
			{
				postfix += operators.top();
				postfix += " ";
				operators.pop();
			}

			operators.push(c); // ����������� ������� ��������
		}

		else throw invalid_argument("����������� ������: " + string(1, c));
	}

	// ������ ���� ���������� ����������
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
	stack<Set> values; // ���� � ������������ �����������
	istringstream pfx(postfix); // ��������� ����� �� ����������� ������
	string token; // ������� ��� ��������

	while (pfx >> token) // ���� �������� �� ���������
	{
		if (isdigit(token[0])) // ���� ����� ���������
		{
			int setNum = stoi(token);
			values.push(sets[setNum - 1]); // ��������� � ����
		}

		else if (token == "!")
		{
			Set operand = values.top(); // ����� ������� �������
			values.pop();
			values.push(operand.Complement()); // ��������� ���������� � ����
		}

		else // ��������� �������� ����������
		{
			Set right = values.top(); // ������ �������
			values.pop();
			Set left = values.top(); //����� �������
			values.pop();


			switch (token[0])
			{
			case '+':
				values.push(left.Union(right)); // � ���� ����������� �����������
				break;
			case '*':
				values.push(left.Intersection(right)); // � ���� ����������� �����������
				break;
			case '-':
				values.push(left.Difference(right)); // � ���� ����������� ��������
				break;
			case '^':
				values.push(left.SymmetricDifference(right)); // � ���� ����������� �������������� ��������
				break;
			}
		}
	}

	// �������� ����������
	if (values.size() != 1)
		throw invalid_argument("������������ ���������");

	return values.top(); // ���������� ��������� ���������
}

Set Parser::Evaluate(const string& expression)
{
	CheckExpression(expression);

	string postfix = InfixToPostfix(expression); // ����������� ��������� � ����������� ������
	return EvaluatePostfix(postfix);
}

void Parser::CheckExpression(const string& expression)
{
	if (expression == "")
		throw invalid_argument("��������� �� ����� ���� ������");

	int bracetBal = 0;
	bool expectOperand = true; // true - ��������� �������, false - ��������� ��������

	for (int i = 0; i < expression.length(); i++)
	{
		char c = expression[i];
		if (isspace(c)) continue;

		if (expectOperand == true) // ���� ��������� �������
		{
			if (isdigit(c))
			{
				string number;
				while (i < expression.length() && isdigit(expression[i]))
				{
					number += expression[i]; // ��������� ��� ����� � �����
					i++;
				}
				i--;

				int setNum = stoi(number);
				if (!(setNum >= 1 && setNum <= sets.size()))
					throw invalid_argument("�������� ����� ���������: " + number);
				
				expectOperand = false; // ������ ��������� ��������
			}
			
			else if (c == '!')
				expectOperand = true; // ����� ! ��������� ��������

			else if (c == '(')
			{
				bracetBal++; // ������ ������ +1
				expectOperand = true; // ����� ( ��������� ��������
			}

			else
				throw invalid_argument("������������ ���������");
		}

		else // ��������� �������� ��������
		{
			if (IsOperator(c) && c != '!')
				expectOperand = true; // ����� �������� �������� ��������� �������

			else if (c == ')')
			{
				bracetBal--;
				if (bracetBal < 0)
					throw invalid_argument("������ ����������� ������");

				expectOperand = false; // ����� ) ��������� ��������
			}

			else
				throw invalid_argument("������������ ���������");
		}
		
	}

	if (expectOperand)
		throw invalid_argument("������������ ���������");

	if (bracetBal > 0)
		throw invalid_argument("���������� ������");
}