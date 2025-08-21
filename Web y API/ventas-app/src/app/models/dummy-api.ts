export interface DummyApiResponse {
  message: string;
  data: DummyData;
  timestamp: string;
  status: number;
}

export interface DummyData {
  id: number;
  name: string;
  email: string;
  company: string;
  phone: string;
  website: string;
  address: {
    street: string;
    city: string;
    zipcode: string;
  };
}