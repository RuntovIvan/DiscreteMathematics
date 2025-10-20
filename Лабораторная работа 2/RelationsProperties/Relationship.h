#pragma once
#include <iostream>
#include <fstream>
using namespace std;

class Relationship
{
	int** matrix;
	static const int size = 6;

private:
	int** BuildMatrix2();

public:
	Relationship();
	~Relationship();
	void ShowMatrix();

	bool IsReflexive();
	bool IsAntireflexive();
	bool IsSymmetric();
	bool IsAntisymmetric();
	bool IsAsymmetric();
	bool IsTransitive();
	bool IsConnective();

	bool IsEquivalence();

	bool IsStrictCompleteOrder();
	bool IsStrictPartialOrder();
	bool IsNonstrictCompleteOrder();
	bool IsNonstrictPartialOrder();
};

Relationship::Relationship()
{
	matrix = new int* [size];
	for (int i = 0; i < size; i++)
		matrix[i] = new int[size] {};

	ifstream file("matrix.txt");
	for (int i = 0; i < size; i++)
		for (int j = 0; j < size; j++)
			file >> matrix[i][j];
}

Relationship::~Relationship()
{
	for (int i = 0; i < size; i++)
		delete[] matrix[i];
	delete[] matrix;
}

void Relationship::ShowMatrix() 
{
	cout << "Матрица отношения:" << endl;
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
			cout << matrix[i][j] << ' ';
		cout << endl;
	}
	cout << endl;
}

bool Relationship::IsReflexive() 
{
	for (int i = 0; i < size; i++)
		if (!matrix[i][i]) // если встретился 0
			return false;
	return true;
}

bool Relationship::IsAntireflexive() 
{
	for (int i = 0; i < size; i++)
		if (matrix[i][i]) // если встретилась 1
			return false;
	return true;
}

bool Relationship::IsSymmetric() 
{
	for (int i = 0; i < size; i++)
		for (int j = i + 1; j < size; j++)
			if (matrix[i][j] != matrix[j][i]) // если не равны
				return false;
	return true;
}

bool Relationship::IsAntisymmetric() 
{
	for (int i = 0; i < size; i++)
		for (int j = i + 1; j < size; j++)
			// если встретилась 1, и на обратную связь тоже 1, то не проходит
			if (matrix[i][j] && matrix[j][i]) 
				return false;
	return true;
}

bool Relationship::IsAsymmetric()
{
	// антирефлексивность и антисимметричность
	return IsAntireflexive() && IsAntisymmetric(); 
}

int** Relationship::BuildMatrix2()
{
	int** matrix2 = new int* [size];
	for (int i = 0; i < size; i++)
		matrix2[i] = new int[size] {};

	for (int row = 0; row < size; row++)
		for (int col = 0; col < size; col++)
			for (int k = 0; k < size; k++)
				if (matrix[row][k] && matrix[k][col])
					matrix2[row][col] = 1;

	return matrix2;
}

bool Relationship::IsTransitive() 
{
	int** matrix2 = BuildMatrix2();

	for (int i = 0; i < size; i++)
		for (int j = 0; j < size; j++)
			if (matrix2[i][j] == 1 && matrix[i][j] == 0) // если можно пройти через одну точку, но нельзя напрямую
				return false;
	return true;
}

bool Relationship::IsConnective()
{
	for (int i = 0; i < size; i++)
		for (int j = i + 1; j < size; j++)
			if (matrix[i][j] + matrix[j][i] == 0)
				return false;
	return true;
}

bool Relationship::IsEquivalence()
{
	return IsReflexive() && IsSymmetric() && IsTransitive();
}

// строгий полный
bool Relationship::IsStrictCompleteOrder()
{
	return IsAntisymmetric() && IsTransitive() && IsAntireflexive() && IsConnective();
}

// строгий частичный
bool Relationship::IsStrictPartialOrder()
{
	return IsAntisymmetric() && IsTransitive() && IsAntireflexive();
}

// нестрогий полный
bool Relationship::IsNonstrictCompleteOrder()
{
	return IsAntisymmetric() && IsTransitive() && IsReflexive() && IsConnective();
}

// нестрогий частичный
bool Relationship::IsNonstrictPartialOrder()
{
	return IsAntisymmetric() && IsTransitive() && IsReflexive();
}