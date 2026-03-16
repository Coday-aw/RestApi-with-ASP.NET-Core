namespace E_CommerceApi.DTOs.Product;

public record PaginationMetaDto(
    int Page,
    int PageSize,
    int TotalPages,
    int TotalCount,
    bool HasNext,
    bool HasPrevious
);

