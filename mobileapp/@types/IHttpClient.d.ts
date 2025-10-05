export interface HttpClient {
	get: (url: string, config?: any) => Promise<any>;
	post: (url: string, config?: any, options?: any) => Promise<any>;
	put: (url: string, config?: any, options?: any) => Promise<any>;
	delete: (url: string, config?: any) => Promise<any>;
}