#pragma once

#include <iostream>
#include <filesystem>

#include <map>
#include <string>

#include "Core/Parser.hpp"
#include "Core/Constructor.hpp"
#include "Core/Exporter.hpp"

bool EntryPoint(const std::filesystem::path* const InputPath, const std::filesystem::path* const OutputPath);