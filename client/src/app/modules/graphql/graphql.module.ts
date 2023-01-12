import { NgModule } from '@angular/core';
import { ApolloModule, APOLLO_OPTIONS } from 'apollo-angular';
import { ApolloClientOptions, from, InMemoryCache } from '@apollo/client/core';
import { HttpLink } from 'apollo-angular/http';
import { environment } from '../../../environments/environment';

export function createApollo(httpLink: HttpLink): ApolloClientOptions<any> {
  const httpLinkHandler = httpLink.create({
    uri: environment.graphQL,
    includeExtensions: true
  });

  return {
    link: from([
      httpLinkHandler
    ]),
    cache: new InMemoryCache()
  };
}

@NgModule({
  exports: [
    ApolloModule
  ],
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory: createApollo,
      deps: [
        HttpLink
      ]
    }
  ]
})
export class GraphQLModule {
}
