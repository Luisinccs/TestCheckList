
using TestCheckList.Models;

namespace TestCheckList.ViewModels;

public record TaskItemDto(int Index, string Titulo, TaskState Estado);