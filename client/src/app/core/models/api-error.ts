export interface IApiError {
  errors: { [key: string]: string[] };
  genericErrors: string[];
}
