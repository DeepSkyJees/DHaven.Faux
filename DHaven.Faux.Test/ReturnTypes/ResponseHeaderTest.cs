﻿#region Copyright 2018 D-Haven.org
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DHaven.Faux.HttpSupport;
using FluentAssertions;
using Xunit;

namespace DHaven.Faux.Test.ReturnTypes
{
    public class ResponseHeaderTest
    {
        [Fact]
        public async Task AsyncReceiveResponseHeader()
        {
            var service = Test.FauxReturn.Service;

            DiscoverySupport.Client = Test.MockRequest(request =>
                    request.Method == HttpMethod.Put && request.RequestUri.ToString() == "http://return/",
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Headers = {Location = new Uri("http://return/abcd-1234")}
                });

            var location = await service.PutAsync();
            location.Should().BeEquivalentTo("http://return/abcd-1234");
        }

        [Fact]
        public void ReceiveResponseHeader()
        {
            var service = Test.FauxReturn.Service;

            DiscoverySupport.Client = Test.MockRequest(request =>
                    request.Method == HttpMethod.Put && request.RequestUri.ToString() == "http://return/",
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Headers = {Location = new Uri("http://return/abcd-1234")}
                });

            var location = service.Put();
            location.Should().BeEquivalentTo("http://return/abcd-1234");
        }

        [Fact]
        public async Task AsyncReceiveJsonSerializedObject()
        {
            var service = Test.FauxReturn.Service;

            DiscoverySupport.Client = Test.MockRequest(request =>
                    request.Method == HttpMethod.Post && request.RequestUri.ToString() == "http://return/echo",
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{Content: \"Hello World\", IsValid: true}", Encoding.UTF8,
                        "application/json")
                });

            var value = await service.EchoAsync(new Value {Content = "Goodbye", IsValid = false});
            value.Content.Should().BeEquivalentTo("Hello World");
            value.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ReceiveJsonSerializedObject()
        {
            var service = Test.FauxReturn.Service;

            DiscoverySupport.Client = Test.MockRequest(request =>
                    request.Method == HttpMethod.Post && request.RequestUri.ToString() == "http://return/echo",
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{Content: \"Hello World\", IsValid: true}", Encoding.UTF8,
                        "application/json")
                });

            var value = service.Echo(new Value {Content = "Goodbye", IsValid = false});
            value.Content.Should().BeEquivalentTo("Hello World");
            value.IsValid.Should().BeTrue();
        }
    }
}