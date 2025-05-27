// src/nodes/index.ts
import httpRequest from './httpRequest';
import csvReader   from './csvReader';

export const nodeRegistry: Record<string, NodeDefinition> = {
  [httpRequest.id]: httpRequest,
  [csvReader.id]:   csvReader,
};
