#include <iostream>

#include "Application.hpp"

int main(int argc, char* argv[])
{
	std::filesystem::path input;
	std::filesystem::path output;

	// Clear trash memory
	{
		std::filesystem::path path(argv[0]);
		path = path.parent_path();

		input = path / "Input";
		output = path / "Output";
	}

	if (!std::filesystem::exists(input))
	{
		if (!std::filesystem::create_directory(input))
		{
			std::cout << "FAILURE\n";
			std::cin.get();

			return -1;
		}
	}

	if (!std::filesystem::exists(output))
	{
		if (!std::filesystem::create_directory(output))
		{
			std::cout << "FAILURE\n";
			std::cin.get();

			return -1;
		}
	}

	if (EntryPoint(&input, &output))
	{
		std::cout << "SUCCESS\n";
		std::cin.get();

		return 0;
	}
	else
	{
		std::cout << "FAILURE\n";
		std::cin.get();

		return -1;
	}
}