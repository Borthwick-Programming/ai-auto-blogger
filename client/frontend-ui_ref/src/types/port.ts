// very small placeholder until we design ports properly
export interface Port {
  id: string;
  label?: string;
  dataType?: string;   // e.g. "json", "string", later used for validation
}
