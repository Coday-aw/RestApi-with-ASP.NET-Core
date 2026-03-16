namespace E_CommerceApi.DTOs.Product;

public record PagedResponse<T> (
    IEnumerable<T> Data,
    PaginationMetaDto Pagination
);