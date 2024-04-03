export interface PagedResult<T> {
  page: number;
  items: T[];
  totalPages: number;
  totalResults: number;
}
